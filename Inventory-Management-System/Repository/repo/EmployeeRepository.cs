
namespace Inventory_Management_System.Repository.repo
{
    public class EmployeeRepository : IEmployeeRepository
    {
        ApplicationDbContext context;
        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }
        public void Add(Employee entity)
        {
            context.Add(entity);
        }

        public void Delete(Employee entity)
        {
            context.Remove(entity);
        }

        public void DeleteEmployees(List<int> employeeIds)
        {
            foreach (int employeeId in employeeIds)
            {
                Employee employee = GetById(employeeId);
                Delete(employee);
            }
            Save(); 
        }

        public List<Employee> GetAll()
        {
            return context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return context.Employees.FirstOrDefault(e => e.ID == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Employee entity)
        {
            context.Update(entity);
        }

        public List<Employee> GetByName(string name)
        {
            return context.Employees.Where(d => d.FName == name).ToList();
        }
    }
}
