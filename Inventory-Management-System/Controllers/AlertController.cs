using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class AlertController : Controller
    {
        private readonly IAlertRepository alertRepository;

        public AlertController(IAlertRepository alertRepository) 
        {
            this.alertRepository = alertRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
