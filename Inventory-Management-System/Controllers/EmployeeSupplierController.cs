using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class EmployeeSupplierController : Controller
    {
        private readonly IEmployeeSupplierRepository employeeSupplierRepository;
        public EmployeeSupplierController(IEmployeeSupplierRepository employeeSupplier)
        {
            this.employeeSupplierRepository = employeeSupplier;
        }
        public IActionResult Index()
        {
            List<EmployeeSupplier> employeeSuppliers = employeeSupplierRepository.GetAll();
            return View("Index", employeeSuppliers);
        }

        [HttpGet]
        public IActionResult Add()
        {
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
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id != null)
            {
                EmployeeSupplier EmpModel = employeeSupplierRepository.GetById(id);
                return View("Edit", EmpModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult SaveEdit(EmployeeSupplier employeeFromRequest, int id)
        {
            if (ModelState.IsValid)
            {
                EmployeeSupplier EmpFromDB = employeeSupplierRepository.GetById(id);
                EmpFromDB.ProductIdentifier = employeeFromRequest.ProductIdentifier;
                EmpFromDB.StartDate = DateTime.Now;
                EmpFromDB.TotalCost = employeeFromRequest.TotalCost;
                EmpFromDB.Quantity = employeeFromRequest.Quantity;
                EmpFromDB.SupplierID = employeeFromRequest.SupplierID;
                EmpFromDB.EmployeeID = employeeFromRequest.EmployeeID;
                employeeSupplierRepository.Update(EmpFromDB);
                employeeSupplierRepository.Save();
                return RedirectToAction("Index");
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
