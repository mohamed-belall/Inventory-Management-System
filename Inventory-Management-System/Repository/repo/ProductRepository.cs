
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
            return _applicationDbContext.Products.ToList();
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
