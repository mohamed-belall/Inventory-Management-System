
namespace Inventory_Management_System.Repository.repo
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EmployeeRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public void Add(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Employee entity)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetAll()
        {
            throw new NotImplementedException();
        }

        public Employee GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
