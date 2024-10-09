using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository) 
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index(int? id)
        {
            List<Product> products;
            products = productRepository.GetFilteredByCategory(id).ToList();

            ViewBag.categories = categoryRepository.GetAll().ToList();
            ViewBag.selectedCategoryId = id ;
            return View(products);
        }

        public IActionResult Search(string name,int? id)
        {
            var products = productRepository.GetFilteredByNameWithCategory(name,id).ToList();

            ViewBag.categories = categoryRepository.GetAll().ToList();
            ViewBag.selectedCategoryId = id;
            return View("Index",products);
        }
    }
}
