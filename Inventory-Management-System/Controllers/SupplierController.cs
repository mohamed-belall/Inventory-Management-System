using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ISupplierRepository supplierRepository;

        public SupplierController(ApplicationDbContext _Context, ISupplierRepository supplierRepository)
        {
            applicationDbContext = _Context;
            this.supplierRepository = supplierRepository;
        }

        public IActionResult Index()
        {
            var suppliers = supplierRepository.GetAll();
            return View("Index", suppliers);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult SaveAdd(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplierRepository.Add(supplier);
                supplierRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Add", supplier);
        }

        public IActionResult Edit(int id)
        {
            var supplier = supplierRepository.GetById(id);
            if (supplier != null)
            {
                return View("Edit" , supplier);
            }
            return Content("This Id Not Found");
        }

        [HttpPost]
        public IActionResult SaveEdit(Supplier supplier , int id)
        {
            if (ModelState.IsValid)
            {
                var existingSupplier = supplierRepository.GetById(id);
                if (existingSupplier != null)
                {
                    // Update the properties of the existing supplier with the new values
                    existingSupplier.Name = supplier.Name;
                    existingSupplier.Email = supplier.Email;
                    existingSupplier.Address = supplier.Address;
                    existingSupplier.Phone = supplier.Phone;
                    supplierRepository.Update(existingSupplier);
                    supplierRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            return View("Edit", supplier);
        }

        public IActionResult Delete(int id)
        {
            var supplier = supplierRepository.GetById(id);
            if (supplier != null)
            {
                supplierRepository.Delete(supplier);
                supplierRepository.Save();
                return RedirectToAction("Index");
            }
            return Content("This Id Not found");
        }

        public IActionResult SearchByName(string name)
        {
            ViewBag.SearchItem = name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound("Please enter a valid name to search.");
            }

            var Supplier = supplierRepository.SearchByName(name);
            if (Supplier != null)
            {
                return View("Index", Supplier);
            }
            else
            {
                return NotFound("Instructor with the given name not found.");
            }
        }
    }
}
