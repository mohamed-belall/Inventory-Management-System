
namespace Inventory_Management_System.Repository.repo
{
    public class EmployeeSupplierRepository : IEmployeeSupplierRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EmployeeSupplierRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }


        public void Add(EmployeeSupplier entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(EmployeeSupplier entity)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeSupplier> GetAll()
        {
            throw new NotImplementedException();
        }

        public EmployeeSupplier GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(EmployeeSupplier entity)
        {
            throw new NotImplementedException();
        }
    }
}
