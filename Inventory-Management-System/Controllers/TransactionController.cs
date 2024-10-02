using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository) 
        {
            this.transactionRepository = transactionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
