using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
using Microsoft.AspNetCore.Mvc;

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
                    //date updated
                    employeeSupplier.StartDate = DateTime.Now;

                    //if there was a change in the product
                    if ((employeeSupplier.Quantity != employeeFromRequest.Quantity) || (employeeSupplier.TotalCost != employeeFromRequest.TotalCost))
                    {
                        Product? product = _productRepository.GetById(employeeFromRequest.ProductIdentifier);
                        if (product != null)
                        {
                            //if there was a change in the unit cost
                            if (employeeSupplier.TotalCost != employeeFromRequest.TotalCost)
                            {
                                product.UnitPrice = (employeeFromRequest.TotalCost / employeeFromRequest.Quantity) * 1.05;
                                employeeSupplier.TotalCost = employeeFromRequest.TotalCost;
                            }
                            //if there was a change in the Quantity
                            //            old                            new
                            if (employeeSupplier.Quantity != employeeFromRequest.Quantity)
                            {
                                //the user increases the quantity
                                if(employeeFromRequest.Quantity > employeeSupplier.Quantity)
                                {
                                    product.StockQuantity += (employeeFromRequest.Quantity - employeeSupplier.Quantity);
                                    employeeSupplier.Quantity = employeeFromRequest.Quantity;
                                }
                                //the user decreases the quantity
                                else
                                {
                                    if (((employeeFromRequest.Quantity - employeeSupplier.Quantity) + product.StockQuantity) > 0)
                                    {
                                        //the ordered quantity were large by mistake but can be retreived
                                        product.StockQuantity += (employeeFromRequest.Quantity - employeeSupplier.Quantity);
                                        employeeSupplier.Quantity = employeeFromRequest.Quantity;
                                    }
                                    else
                                    {
                                        //the ordered quantity were large by mistake but they where already sold
                                        //you can only edit the quantity to what only was left
                                        employeeSupplier.Quantity = (employeeSupplier.Quantity - product.StockQuantity);
                                        product.StockQuantity -= employeeSupplier.Quantity;
                                    }
                                }
                                //update product
                                product.ModifiedDate = DateTime.Now;
                                _productRepository.Update(product);
                                _productRepository.Save();

                                

                                StartAlert? startAlert = alertRepository.GetByProductId(employeeFromRequest.ProductIdentifier);
                                if (startAlert != null)
                                {
                                    //Check if quantity less than the threshold and alert
                                    if ((product.StockQuantity > product.ReorderLevel)||(startAlert.IsResolved == false))
                                    {
                                        startAlert.IsResolved = true;
                                        startAlert.ResolveDate = DateTime.Now;
                                        alertRepository.Update(startAlert);
                                        alertRepository.Save();
                                    }
                                    else if ((product.StockQuantity < product.ReorderLevel) || (startAlert.IsResolved == false))
                                    {

                                    }

                                }

                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Product not found");
                        }

                    }

                    

                    //info must be updated in the receipt

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
    }
}
