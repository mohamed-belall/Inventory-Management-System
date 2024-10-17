using Inventory_Management_System.Models;
using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Inventory_Management_System.Controllers
{
    public class EmployeeSupplierController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeSupplierRepository employeeSupplierRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IAlertRepository alertRepository;
        public EmployeeSupplierController(IEmployeeSupplierRepository employeeSupplier, IProductRepository productRepository, ISupplierRepository supplierRepository, IEmployeeRepository employeeRepository, IAlertRepository alertRepository)
        {
            this.employeeSupplierRepository = employeeSupplier;
            this._productRepository = productRepository;
            this.supplierRepository = supplierRepository;
            this.employeeRepository = employeeRepository;
            this.alertRepository = alertRepository;
        }
        public IActionResult Index()
        {
            List<EmployeeSupplier> employeeSuppliers = employeeSupplierRepository.GetAll();
            List<string> productNames = new List<string>();
            Product product = new Product();
            foreach (var item in employeeSuppliers)
            {
                product = _productRepository.GetById(item.ProductIdentifier);
                productNames.Add(product.Name);
            }

            ViewBag.Products = productNames;
            return View("Index", employeeSuppliers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Productlist"] = _productRepository.GetAll();
            ViewData["Employeelist"] = employeeRepository.GetAll();
            ViewData["Supplierlist"] = supplierRepository.GetAll();
            return View("Add");
        }

        [HttpPost]
        public IActionResult SaveAdd(EmployeeSupplier employeeFromRequest)
        {
            if (ModelState.IsValid)
            {
                employeeFromRequest.StartDate = DateTime.Now;
                employeeSupplierRepository.Add(employeeFromRequest);
                employeeSupplierRepository.Save();
                Product? product = _productRepository.GetById(employeeFromRequest.ProductIdentifier);
                if(product !=null)
                {
                    //update product
                    product.StockQuantity += employeeFromRequest.Quantity;
                    product.UnitPrice = (employeeFromRequest.TotalCost/employeeFromRequest.Quantity)*1.05;
                    product.ModifiedDate = DateTime.Now;
                    _productRepository.Update(product);
                    _productRepository.Save();

                    if (product.StockQuantity > product.ReorderLevel)
                    {
                        StartAlert? startAlert = alertRepository.GetByProductId(employeeFromRequest.ProductIdentifier);
                        if (startAlert != null)
                        {
                            startAlert.IsResolved = true;
                            startAlert.ResolveDate = DateTime.Now;
                            alertRepository.Update(startAlert);
                            alertRepository.Save();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Product not found");
                }
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id != null)
            {
                EmployeeSupplier employeeSupplier = employeeSupplierRepository.GetById(id);
                EmployeeSupplierWithSupplierList EmpSupViewModel = new EmployeeSupplierWithSupplierList();
                
                EmpSupViewModel.Id = employeeSupplier.Id;
                EmpSupViewModel.TotalCost = employeeSupplier.TotalCost;
                EmpSupViewModel.Quantity = employeeSupplier.Quantity;
                EmpSupViewModel.EmployeeID = employeeSupplier.EmployeeID;
                EmpSupViewModel.SupplierID = employeeSupplier.SupplierID;
                EmpSupViewModel.ProductIdentifier = employeeSupplier.ProductIdentifier;
                

                EmpSupViewModel.Employees = employeeRepository.GetAll(); ;
                EmpSupViewModel.Suppliers = supplierRepository.GetAll();
                EmpSupViewModel.Products = _productRepository.GetAll();

                return View("Edit", EmpSupViewModel);
            }
            else
            {
                return View("Error");
            }

        }

        [HttpPost]
        public IActionResult SaveEdit(EmployeeSupplierWithSupplierList employeeFromRequest)
        {
            if (ModelState.IsValid)
            {
                EmployeeSupplier? employeeSupplier = employeeSupplierRepository.GetById(employeeFromRequest.Id);
                if (employeeSupplier != null)
                {
                    employeeSupplier.StartDate = DateTime.Now;
                    employeeSupplier.TotalCost = employeeFromRequest.TotalCost;
                    employeeSupplier.Quantity = employeeFromRequest.Quantity;
                    employeeSupplier.SupplierID = employeeFromRequest.SupplierID;
                    employeeSupplier.EmployeeID = employeeFromRequest.EmployeeID;
                    employeeSupplier.ProductIdentifier = employeeFromRequest.ProductIdentifier;

                    employeeSupplierRepository.Update(employeeSupplier);
                    employeeSupplierRepository.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Product not found");
                }
            }
            return View("Edit", employeeFromRequest);
        }

        [HttpPost]
        public IActionResult Delete(List<int> employeeIds)
        {
            EmployeeSupplierWithIdListViewModel employeeWithId = new EmployeeSupplierWithIdListViewModel();
            employeeWithId.employeeSuppliers = new List<EmployeeSupplier>();
            employeeWithId.employeeSupplierIds = employeeIds;
            foreach (int id in employeeIds)
            {
                EmployeeSupplier emp = new EmployeeSupplier();
                emp = employeeSupplierRepository.GetById(id);
                if (emp != null)
                {
                    employeeWithId.employeeSuppliers.Add(emp);
                }
            }
            return View("Delete", employeeWithId);
        }

        //[HttpPost]
        //public IActionResult ddeleteConfirmed(int id)
        //{
        //    Employee employee = new Employee();
        //    employee = employeeSupplierRepository.GetById(id);
        //    employeeSupplierRepository.Delete(employee);
        //    employeeSupplierRepository.Save();
        //    return View("deleteConfirmed", employee);
        //}

        [HttpPost]
        public IActionResult deleteConfirmed(List<int> employeeIds)
        {
            List<EmployeeSupplier> employeeList = new List<EmployeeSupplier>();
            foreach (int id in employeeIds)
            {
                EmployeeSupplier emp = new EmployeeSupplier();
                emp = employeeSupplierRepository.GetById(id);
                if (emp != null)
                {
                    employeeList.Add(emp);
                }
            }
            // Check the received IDs and perform deletion logic
            if (employeeIds != null && employeeIds.Any())
            {
                // Example: Delete employees by their IDs from the database
                employeeSupplierRepository.DeleteEmployeeSuppliers(employeeIds);
                return View("deleteConfirmed", employeeList);  // Redirect back to the employee list
            }
            return View("Error");  // Handle the case where no IDs are passed
        }

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            string FullName;
            // Fetch the data to export
            var employeeSuppliers = employeeSupplierRepository.GetAll();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create a worksheet
                var worksheet = package.Workbook.Worksheets.Add("Employee Suppliers");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Employee Name";
                worksheet.Cells[1, 3].Value = "Supplier Name";
                worksheet.Cells[1, 4].Value = "Product Name";
                worksheet.Cells[1, 5].Value = "Quantity";
                worksheet.Cells[1, 6].Value = "Total Cost";
                worksheet.Cells[1, 7].Value = "Start Date";


                // Add data to the worksheet
                for (int i = 0; i < employeeSuppliers.Count; i++)
                {
                    var employeeSupplier = employeeSuppliers[i];
                    // Get the product details
                    var product = _productRepository.GetById(employeeSupplier.ProductIdentifier);
                    string productName = product != null ? product.Name : "Product Not Found";

                    worksheet.Cells[i + 2, 1].Value = employeeSupplier.Id;
                    FullName = employeeSupplier.Employee.FName + "_" + employeeSupplier.Employee.LName;
                    worksheet.Cells[i + 2, 2].Value = FullName;
                    worksheet.Cells[i + 2, 3].Value = employeeSupplier.Supplier.Name;
                    worksheet.Cells[i + 2, 4].Value = productName;
                    worksheet.Cells[i + 2, 5].Value = employeeSupplier.Quantity;
                    worksheet.Cells[i + 2, 6].Value = employeeSupplier.TotalCost;
                    worksheet.Cells[i + 2, 7].Value = employeeSupplier.StartDate.ToString("yyyy-MM-dd");
                }

                // Format the header
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Auto-fit columns for all cells
                worksheet.Cells.AutoFitColumns();

                // Convert to a byte array and return as a file
                var fileContents = package.GetAsByteArray();
                var fileName = "EmployeeSuppliers.xlsx";
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        } 
    }
}
