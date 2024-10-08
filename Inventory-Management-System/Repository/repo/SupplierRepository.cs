
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

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

        public void Update(Supplier entity)
        {
            applicationDbContext.Update(entity);
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
            return applicationDbContext.Suppliers.FirstOrDefault(s => s.ID == id)   ;
        }

        public List<Supplier> SearchByName(string name)
        {
            return applicationDbContext.Suppliers.Where(i => i.Name.Contains(name)).ToList();
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }
    }
}
