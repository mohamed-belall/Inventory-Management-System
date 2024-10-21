using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {

            List<Category> categories = categoryRepository.GetAll();
            return View("Index", categories);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View("Add");
        }

        [HttpPost]
        public IActionResult SaveAdd(Category categoryFromRequest)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(categoryFromRequest);
                categoryRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Add");
        }

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    if (id != null)
        //    {
        //        Employee EmpModel = employeeRepository.GetById(id);
        //        return View("Edit", EmpModel);
        //    }
        //    else
        //    {
        //        return View("Error");
        //    }
        //}

        //[HttpPost]
        //public IActionResult SaveEdit(Employee employeeFromRequest, int id)
        //{
        //    if ((employeeFromRequest.FName != null) && (employeeFromRequest.LName != null) && (ModelState.IsValid))
        //    {
        //        Employee EmpFromDB = employeeRepository.GetById(id);
        //        EmpFromDB.Email = employeeFromRequest.Email;
        //        EmpFromDB.CreatedDate = DateTime.Now;
        //        EmpFromDB.FName = employeeFromRequest.FName;
        //        EmpFromDB.LName = employeeFromRequest.LName;
        //        EmpFromDB.Phone = employeeFromRequest.Phone;
        //        employeeRepository.Update(EmpFromDB);
        //        employeeRepository.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View("Edit", employeeFromRequest);
        //}

        //[HttpPost]
        //public IActionResult DeleteSelected(List<int> employeeIds)
        //{
        //    EmployeeWithIdListViewModel employeeWithId = new EmployeeWithIdListViewModel();
        //    employeeWithId.Employees = new List<Employee>();
        //    employeeWithId.employeeIds = employeeIds;
        //    foreach (int id in employeeIds)
        //    {
        //        Employee emp = new Employee();
        //        emp = employeeRepository.GetById(id);
        //        if (emp != null)
        //        {
        //            employeeWithId.Employees.Add(emp);
        //        }
        //    }
        //    return View("Delete", employeeWithId);
        //}

        //[HttpPost]
        //public IActionResult ddeleteConfirmed(int id)
        //{
        //    Employee employee = new Employee();
        //    employee = employeeRepository.GetById(id);
        //    employeeRepository.Delete(employee);
        //    employeeRepository.Save();
        //    return View("deleteConfirmed", employee);
        //}

        //[HttpGet]
        //public IActionResult Search(string StringFromRequest)
        //{
        //    if (StringFromRequest == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View("Index", categoryRepository.GetByName(StringFromRequest));
        //}
    }
}
