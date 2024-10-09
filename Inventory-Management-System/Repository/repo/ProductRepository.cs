
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repository.repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext) 
        {
            this._applicationDbContext = applicationDbContext;
        }

        public void Add(Product product)
        {
            _applicationDbContext.Products.Add(product); 
        }

        public void Delete(Product product)
        {
             _applicationDbContext.Products.Remove(product);
        }

        public List<Product> GetAll()
        {
            return _applicationDbContext.Products.Include(p=>p.category).Include(p=>p.supplier).ToList();
        }

        public List<Product> GetFilteredByCategory(int? id)
        {
            if (id == null)
                return GetAll();
            else
                return _applicationDbContext.Products.Where(p=>p.CategoryId==id).Include(p=>p.supplier).ToList();
        }

        public List<Product> GetFilteredByName(string name)//helper method
        {
            return _applicationDbContext.Products.Where(p => p.Name.Contains(name) ).Include(p => p.supplier).ToList();
        }

        public List<Product> GetFilteredByNameWithCategory(string name,int? id)
        {
            if (id == null)
                return GetFilteredByName(name);
            else
                return _applicationDbContext.Products.Where(p => p.Name.Contains(name)&& p.CategoryId==id ).Include(p => p.supplier).ToList();
        }

        public Product GetById(int id)
        {
            return _applicationDbContext.Products.FirstOrDefault(p => p.ID == id)!;
        }

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            _applicationDbContext.Products.Update(product);
        }
    }
}
