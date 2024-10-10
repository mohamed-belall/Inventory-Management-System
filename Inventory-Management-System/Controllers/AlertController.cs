using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class AlertController : Controller
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IProductRepository _productRepo;

        public AlertController(IAlertRepository alertRepo , IEmployeeRepository empRepo , IProductRepository productRepo) 
        {
            this._alertRepo = alertRepo;
            this._empRepo = empRepo;
            this._productRepo = productRepo;
        }

        public IActionResult Index()
        {
            List<StartAlert> startAlerts =  _alertRepo.GetAlertWithAllData();

          
            ViewBag.status = "";

            return View("index" , startAlerts);
        }
    
        public IActionResult Add()
        {
            AlertWithEmployeesProductViewModel viewModel = new AlertWithEmployeesProductViewModel()
            {
                
                Employees = _empRepo.GetAll(),
                Products = _productRepo.GetAll(),
                AlertDate = DateTime.Now,
            };

            return View("Add" , viewModel);
        }

        [HttpPost]
        public IActionResult SaveAdd(AlertWithEmployeesProductViewModel newAlertFReq)
        { 

            if(ModelState.IsValid)
            {
                StartAlert startAlert = new StartAlert()
                {
                    AlertDate = newAlertFReq.AlertDate,
                    AlertQuantityLevel = newAlertFReq.AlertQuantityLevel,
                    IsResolved = newAlertFReq.IsResolved,
                    EmployeeId = newAlertFReq.EmployeeId,
                    ProductId = newAlertFReq.ProductId,
                };

               _alertRepo.Add(startAlert);
                _alertRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Add", newAlertFReq);
            
        }


        public IActionResult Update(int id)
        {
            StartAlert startAlert = _alertRepo.GetById(id); 
            AlertWithEmployeesProductViewModel viewModel = new AlertWithEmployeesProductViewModel()
            {
                ID = startAlert.ID,
                AlertQuantityLevel = startAlert.AlertQuantityLevel,
                EmployeeId = startAlert.EmployeeId,
                IsResolved = startAlert.IsResolved,
                ProductId = startAlert.ProductId,
                ResolveDate = startAlert.ResolveDate.GetValueOrDefault(),
                Employees = _empRepo.GetAll(),
                Products = _productRepo.GetAll(),
                AlertDate = DateTime.Now,
            };

            return View("Update", viewModel);
        }

        [HttpPost]
        public IActionResult SaveUpdate(int id ,AlertWithEmployeesProductViewModel updatedAlertFReq)
        {

            StartAlert alertFdb = _alertRepo.GetById(id);
            if (ModelState.IsValid)
            {
               
                if (alertFdb.AlertQuantityLevel != updatedAlertFReq.AlertQuantityLevel )
                    alertFdb.AlertQuantityLevel = updatedAlertFReq.AlertQuantityLevel;

                if (alertFdb.IsResolved != updatedAlertFReq.IsResolved)
                    alertFdb.IsResolved = updatedAlertFReq.IsResolved;

                if (alertFdb.EmployeeId != updatedAlertFReq.EmployeeId)
                    alertFdb.EmployeeId = updatedAlertFReq.EmployeeId;

                if (alertFdb.ProductId != updatedAlertFReq.ProductId)
                    alertFdb.ProductId = updatedAlertFReq.ProductId;


                if (updatedAlertFReq.IsResolved)
                    alertFdb.ResolveDate = DateTime.Now;


                _alertRepo.Update(alertFdb);
                _alertRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Add", updatedAlertFReq);

        }


        public IActionResult Search(string name , string status)
        {
            List<StartAlert> startAlerts = _alertRepo.GetSearchAndStatusResult( name , status);


            if (status == "Pending")
                ViewBag.status = "Pending";
            else if (status == "Completed")
                ViewBag.status = "Completed";
            else
                ViewBag.status = "";

            return View("index", startAlerts);
        }

    }



    
}
