using Inventory_Management_System.Repository;
using Inventory_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            List<Employee> employees = employeeRepository.GetAll();
            return View("Index", employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult SaveAdd(Employee employeeFromRequest)
        {
            if ((employeeFromRequest.FName != null) && (employeeFromRequest.LName != null) && (ModelState.IsValid))
            {
                employeeFromRequest.CreatedDate = DateTime.Now;
                employeeRepository.Add(employeeFromRequest);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id != null)
            {
                Employee EmpModel = employeeRepository.GetById(id);
                return View("Edit", EmpModel);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult SaveEdit(Employee employeeFromRequest, int id)
        {
            if ((employeeFromRequest.FName != null) && (employeeFromRequest.LName != null) && (ModelState.IsValid))
            {
                Employee EmpFromDB = employeeRepository.GetById(id);
                EmpFromDB.Email = employeeFromRequest.Email;
                EmpFromDB.CreatedDate = DateTime.Now;
                EmpFromDB.FName = employeeFromRequest.FName;
                EmpFromDB.LName = employeeFromRequest.LName;
                EmpFromDB.Phone = employeeFromRequest.Phone;
                employeeRepository.Update(EmpFromDB);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", employeeFromRequest);
        }

        [HttpPost]
        public IActionResult Delete(List<int> employeeIds)
        {
            EmployeeWithIdListViewModel employeeWithId = new EmployeeWithIdListViewModel();
            employeeWithId.Employees = new List<Employee>();
            employeeWithId.employeeIds = employeeIds;
            foreach (int id in employeeIds)
            {
                Employee emp = new Employee();
                emp = employeeRepository.GetById(id);
                if (emp != null)
                {
                    employeeWithId.Employees.Add(emp);
                }
            }
            return View("Delete", employeeWithId);
        }

        //[HttpPost]
        //public IActionResult ddeleteConfirmed(int id)
        //{
        //    Employee employee = new Employee();
        //    employee = employeeRepository.GetById(id);
        //    employeeRepository.Delete(employee);
        //    employeeRepository.Save();
        //    return View("deleteConfirmed", employee);
        //}

        [HttpPost]
        public IActionResult deleteConfirmed(List<int> employeeIds)
        {
            List<Employee> employeeList = new List<Employee>();
            foreach (int id in employeeIds)
            {
                Employee emp = new Employee();
                emp = employeeRepository.GetById(id);
                if (emp != null)
                {
                    employeeList.Add(emp);
                }
            }
            // Check the received IDs and perform deletion logic
            if (employeeIds != null && employeeIds.Any())
            {
                // Example: Delete employees by their IDs from the database
                employeeRepository.DeleteEmployees(employeeIds);
                return View("deleteConfirmed", employeeList);  // Redirect back to the employee list
            }
            return View("Error");  // Handle the case where no IDs are passed
        }



        [HttpGet]
        public IActionResult Search(string StringFromRequest)
        {
            if (StringFromRequest == null)
            {
                return RedirectToAction("Index");
            }
            return View("Index", employeeRepository.GetByName(StringFromRequest));
        }
    }
}
