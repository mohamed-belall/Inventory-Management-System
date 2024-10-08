using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ISupplierRepository supplierRepository;

        public SupplierController(ApplicationDbContext _Context , ISupplierRepository supplierRepository)
        {
            applicationDbContext = _Context;
            this.supplierRepository = supplierRepository;
        }

        public IActionResult GetAll()
        {
            var suppliers = supplierRepository.GetAll();
            return View("GetAll" , suppliers);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult SaveAdd(Supplier supplier)
        {
            if(ModelState.IsValid)
            {
                supplierRepository.Add(supplier);
                supplierRepository.Save();
                return RedirectToAction("GetAll");
            }
            return View("Add" , supplier);
        }

        public IActionResult Edit(int id)
        {
            var supplier = supplierRepository.GetById(id);
            if(supplier != null)
            {
                return View("Edit");
            }
            return Content("This Id Not Found");
        }

        [HttpPost]
        public IActionResult SaveEdit(Supplier supplier)
        {
            if(ModelState.IsValid)
            {
                supplierRepository.Update(supplier);
                supplierRepository.Save();
                return RedirectToAction("GetAll");
            }
            return View("Edit" , supplier);
        }

        public IActionResult Delete(int id)
        {
            var supplier = supplierRepository.GetById(id);
            if(supplier != null)
            {
                supplierRepository.Delete(supplier);
                supplierRepository.Save();
                return RedirectToAction("GetAll");
            }
            return Content("This Id Not found");
        }
       
    }
}
