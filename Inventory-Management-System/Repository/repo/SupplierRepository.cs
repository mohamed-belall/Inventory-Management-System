
using Microsoft.EntityFrameworkCore;

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
            applicationDbContext.Add(entity);
        }

        public void Delete(Supplier entity)
        {
            applicationDbContext.Remove(entity);
        }

        public List<Supplier> GetAll()
        {
            return applicationDbContext.Suppliers.ToList();
        }

        public Supplier GetById(int id)
        {
            return applicationDbContext.Suppliers.FirstOrDefault(s => s.ID == id);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(Supplier entity)
        {
            applicationDbContext.Update(entity);
        }
        public List<Supplier> SearchByName(string name)
        {
            return applicationDbContext.Suppliers.Where(e => e.Name.Contains(name)).ToList();
        }

        public int GetSupplierCount()
        {
            return applicationDbContext.Suppliers.Count();
        }

        public List<Supplier> GetPagedSuppliers(int page, int pageSize)
        {
            return applicationDbContext.Suppliers
                            .OrderBy(s => s.Name)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
        }
    }
}
