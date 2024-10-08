using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class AlertController : Controller
    {
        private readonly IAlertRepository _alertRepo;

        public AlertController(IAlertRepository alertRepo) 
        {
            this._alertRepo = alertRepo;
        }

        public IActionResult Index()
        {
            List<StartAlert> startAlerts =  _alertRepo.GetAlertWithAllData();
            return View("index" , startAlerts);
        }
    }
}
