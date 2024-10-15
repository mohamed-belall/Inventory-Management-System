using Inventory_Management_System.Models;
using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IEmployeeSupplierRepository employeeSupplierRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ISupplierRepository supplierRepository;

        public HomeController(IProductRepository productRepository ,
                              IEmployeeSupplierRepository employeeSupplierRepository ,
                              IEmployeeRepository employeeRepository ,
                              ISupplierRepository supplierRepository)
        {
            this.productRepository = productRepository;
            this.employeeSupplierRepository = employeeSupplierRepository;
            this.employeeRepository = employeeRepository;
            this.supplierRepository = supplierRepository;
        }

        public ActionResult Index()
        {
            var dashboardCards = new List<DashboardCard>
            {
                new DashboardCard { Title = "Total Items", Value = productRepository.GetItemsCount().ToString(), Icon = "fa-cubes", BackgroundColor = "#6a5acd" },
                new DashboardCard { Title = "Total Categories", Value = "8", Icon = "fa-list-alt", BackgroundColor = "#00bcd4" },
                new DashboardCard { Title = "Total Products", Value = productRepository.GetProductCount().ToString() , Icon = "fa-box-open", BackgroundColor = "#4caf50" },
                new DashboardCard { Title = "Total Orders", Value = employeeSupplierRepository.GetOrdersCount().ToString(), Icon = "fa-handshake", BackgroundColor = "#ff9800" },
                new DashboardCard { Title = "Paid Transactions", Value = "5", Icon = "fa-exchange-alt", BackgroundColor = "#4caf50" },
                new DashboardCard { Title = "Total Sales", Value = "8225.77", Icon = "fa-dollar-sign", BackgroundColor = "#2196f3" },
                new DashboardCard { Title = "Total Supplier", Value = supplierRepository.GetSupplierCount().ToString(), Icon = "fa-store", BackgroundColor = "#f44336" },
                new DashboardCard { Title = "Total Members", Value = employeeRepository.GetEmpCount().ToString(), Icon = "fa-users", BackgroundColor = "#00bcd4" }
            };
            return View(dashboardCards);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
