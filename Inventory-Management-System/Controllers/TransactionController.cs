using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
using Inventory_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductTransactionRepository productTransactionRepository;

        public TransactionController(ITransactionRepository transactionRepository, IProductRepository productRepository, IProductTransactionRepository productTransactionRepository)
        {
            this.transactionRepository = transactionRepository;
            this.productRepository = productRepository;
            this.productTransactionRepository = productTransactionRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Transaction> transactions = transactionRepository.GetAll();
            ViewBag.TopSellingProducts = productTransactionRepository.GetTopSelling();
            ViewBag.TopEmployees = transactionRepository.GetTopEmployees();

            return View("Index", transactions);
        }

        [HttpGet]
        public IActionResult Add()
        {
            List<Product> availableProducts = productRepository.GetAllAvailable();

            TransactionWithProducts transactionWithProducts = new TransactionWithProducts
            {
                AvailableProducts = availableProducts
            };
            return View(transactionWithProducts);
        }

        [HttpPost]
        public IActionResult ShowBill(TransactionWithProducts transactionWithProducts)
        {
            if (!ModelState.IsValid)
            {
                transactionWithProducts.AvailableProducts = productRepository.GetAllAvailable();
                return View("Add", transactionWithProducts);
            }

            List<int> productIds = transactionWithProducts.ProductDetails.Select(p => p.ProductId).ToList();//takes the products ids
            List<Product> products = productRepository.GetByIds(productIds);// fetch the specific products from db

            //check for available quantites 


            transactionWithProducts.AvailableProducts = productRepository.GetAllAvailable();

            double total = 0;
            for (int i = 0; i < products.Count; i++)//calculate the price 
            {
                total += transactionWithProducts.ProductDetails[i].Quantity * products[i].UnitPrice;
            }

            transactionWithProducts.SelectedProducts = products;
            transactionWithProducts.TotalPrice = total;
            return View(transactionWithProducts);
        }

        //list<products> , empid , quantity , total price
        [HttpPost]
        public IActionResult FinalizeTransaction(TransactionWithProducts transactionWithProducts)
        {
            List<int> productIds = transactionWithProducts.ProductDetails.Select(p => p.ProductId).ToList();//takes the products ids
            List<Product> products = productRepository.GetByIds(productIds);// fetch the specific products from db

            for (int i = 0; i < transactionWithProducts.ProductDetails.Count; i++)//decrease quantity from db(update products)
            {
                if (productIds[i] == products[i].ID)
                {
                    products[i].StockQuantity -= transactionWithProducts.ProductDetails[i].Quantity;
                    productRepository.Update(products[i]);
                    productRepository.Save();

                }
            }
            Transaction transaction = new Transaction();
            transaction.EmployeeId = transactionWithProducts.EmployeeId;
            transaction.Date = DateTime.Now;
            transaction.Type = TransactionType.Sale;
            transaction.TotalPrice = transactionWithProducts.TotalPrice ?? 0;

            transactionRepository.Add(transaction);
            transactionRepository.Save();

            int transactionId = transactionRepository.GetLastTransactionId();
            for (int i = 0; i < transactionWithProducts.ProductDetails.Count; i++)//add productTransaction record (many to many relation)
            {
                ProductTransaction productTransaction = new ProductTransaction();
                productTransaction.ProductId = productIds[i];
                productTransaction.TransactionId = transactionId; // realtion with the employee
                productTransaction.Quantity = transactionWithProducts.ProductDetails[i].Quantity;

                productTransactionRepository.Add(productTransaction);
                productTransactionRepository.Save();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(List<int> selectedIds)// this action for admin only to save history of transaction
        {
            foreach (var id in selectedIds)
            {
                Transaction transaction = transactionRepository.GetById(id);
                transactionRepository.Delete(transaction);
                transactionRepository.Save();
            }

            return RedirectToAction("Index");
        }
    }
}
