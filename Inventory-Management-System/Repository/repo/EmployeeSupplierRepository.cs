﻿
using Microsoft.EntityFrameworkCore;

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
            return applicationDbContext.EmployeeSuppliers.Include(e=>e.Employee).Include(e=>e.Supplier).ToList();
        }

        public EmployeeSupplier GetById(int id)
        {
            return applicationDbContext.EmployeeSuppliers.Include(e => e.Employee).Include(e => e.Supplier).FirstOrDefault(e => e.Id == id);
        }

        public EmployeeSupplier GetByDate(DateTime dateTime)
        {
            return applicationDbContext.EmployeeSuppliers.Include(e => e.Employee).Include(e => e.Supplier).FirstOrDefault(e => e.StartDate == dateTime);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(EmployeeSupplier entity)
        {
            applicationDbContext.Update(entity);
        }

        public int GetOrdersCount()
        {
            return applicationDbContext.EmployeeSuppliers.Count();
        }

        public List<EmployeeSupplier> GetPagedReceipt(int page, int pageSize)
        {
            return applicationDbContext.EmployeeSuppliers
                            .OrderBy(s => s.Id)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
        }
    }
}
