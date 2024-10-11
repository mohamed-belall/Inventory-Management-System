
namespace Inventory_Management_System.Repository.repo
{
    public class EmployeeSupplierRepository : IEmployeeSupplierRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EmployeeSupplierRepository(ApplicationDbContext dbContext)
        {
            applicationDbContext = dbContext;
        }
        public void Add(EmployeeSupplier entity)
        {
            applicationDbContext.Add(entity);
        }

        public void Delete(EmployeeSupplier entity)
        {
            applicationDbContext.Remove(entity);
        }

        public void DeleteEmployeeSuppliers(List<int> employeeIds)
        {
            foreach (int employeeId in employeeIds)
            {
                EmployeeSupplier employee = GetById(employeeId);
                Delete(employee);
            }
            Save();
        }

        public List<EmployeeSupplier> GetAll()
        {
            return applicationDbContext.EmployeeSuppliers.ToList();
        }

        public EmployeeSupplier GetById(int id)
        {
            return applicationDbContext.EmployeeSuppliers.FirstOrDefault(e => e.Id == id);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(EmployeeSupplier entity)
        {
            applicationDbContext.Update(entity);
        }

    }
}
