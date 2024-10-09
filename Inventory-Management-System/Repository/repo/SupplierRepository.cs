
namespace Inventory_Management_System.Repository.repo
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SupplierRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public List<Supplier> GetAll()
        {
            return applicationDbContext.Suppliers.ToList();
        }

        public Supplier GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Supplier entity)
        {
            throw new NotImplementedException();
        }
    }
}
