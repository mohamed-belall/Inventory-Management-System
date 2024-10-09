using Inventory_Management_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    public class ProductController : Controller
    {
        const int threshold = 5;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISupplierRepository supplierRepository;

        public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository,ISupplierRepository supplierRepository) 
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
        }
        public IActionResult Index(int? id)
        {
            List<Product> products ;
            products = productRepository.GetFilteredByCategory(id).ToList();

            ViewBag.categories = categoryRepository.GetAll().ToList();
            ViewBag.selectedCategoryId = id;
            return View(products);
        }

        public IActionResult Search(string name,int? id)
        {
            var products = productRepository.GetFilteredByNameWithCategory(name,id).ToList();

            ViewBag.categories = categoryRepository.GetAll().ToList();
            ViewBag.selectedCategoryId = id;
            return View("Index",products);
        }

        public IActionResult Add()//in case of the product will added in first time in my stock
        {
            ProductWithCategoriesViewModel productWithCategories = new ProductWithCategoriesViewModel();
            productWithCategories.categories = categoryRepository.GetAll();
            productWithCategories.suppliers = supplierRepository.GetAll();
            return View(productWithCategories);
        }

        public IActionResult SaveAdd(ProductWithCategoriesViewModel productWithCategories)  
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();

                product.Name = productWithCategories.Name;
                product.UnitPrice = productWithCategories.UnitPrice;
                product.CreatedDate = DateTime.Now;
                product.StockQuantity = productWithCategories.Quantity;
                product.ReorderLevel = threshold;
                product.Description = productWithCategories.Description;
                product.CategoryId = productWithCategories.CategoryId;
                product.SupplierId = productWithCategories.SupplierId;

                productRepository.Add(product);
                productRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Add",productWithCategories);
        }
        [HttpPost]
        public IActionResult Delete(List<int> selectedIds)
        {
            
            foreach (int id in selectedIds)
            {
                Product product = productRepository.GetById(id);
                productRepository.Delete(product);
                productRepository.Save();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Product product = productRepository.GetById(id);
            ProductWithCategoriesViewModel productWithCategories = new ProductWithCategoriesViewModel();

            productWithCategories.ID = product.ID;
            productWithCategories.Name = product.Name;
            productWithCategories.UnitPrice = product.UnitPrice;
            productWithCategories.CreatedDate = product.CreatedDate;
            productWithCategories.Quantity = product.StockQuantity;
            productWithCategories.Description = product.Description;
            productWithCategories.CategoryId = product.CategoryId;
            productWithCategories.SupplierId = product.SupplierId;

            productWithCategories.categories = categoryRepository.GetAll();
            productWithCategories.suppliers = supplierRepository.GetAll();
            
            return View(productWithCategories);
        }

        public IActionResult SaveEdit(ProductWithCategoriesViewModel ProductWithCategoriesViewModel) 
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();

                product.ID = ProductWithCategoriesViewModel.ID??0;
                product.Name = ProductWithCategoriesViewModel.Name;
                product.UnitPrice = ProductWithCategoriesViewModel.UnitPrice;
                product.StockQuantity = ProductWithCategoriesViewModel.Quantity;
                product.Description = ProductWithCategoriesViewModel.Description;
                product.CategoryId = ProductWithCategoriesViewModel.CategoryId;
                product.SupplierId = ProductWithCategoriesViewModel.SupplierId;
                product.CreatedDate = ProductWithCategoriesViewModel.CreatedDate??DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.ReorderLevel = threshold;

                productRepository.Update(product);
                productRepository.Save();

                return RedirectToAction("Index");
            }
            return View("Edit",ProductWithCategoriesViewModel);
        }
    }   
}
