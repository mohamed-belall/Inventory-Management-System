
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repository.repo
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public TransactionRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public void Add(Transaction entity)
        {
            applicationDbContext.Transactions.Add(entity);
        }

        public void Delete(Transaction entity)
        {
            applicationDbContext.Transactions.Remove(entity);
        }

        public List<Transaction> GetAll()
        {
            return applicationDbContext.Transactions.ToList();
        }

        public Transaction GetById(int id)
        {
            return applicationDbContext.Transactions.FirstOrDefault(t => t.ID == id);
        }

        public int GetLastTransactionId()
        {
            return applicationDbContext.Transactions.OrderByDescending(t => t.ID).FirstOrDefault().ID;
        }
        public List<TopEmployee> GetTopEmployees()
        {
            return applicationDbContext.Transactions.Include(t => t.employee).GroupBy(t => t.EmployeeId)
                .Select(e => new TopEmployee
                {
                    EmpId = e.Key,
                    EmpName = (e.FirstOrDefault().employee.FName + e.FirstOrDefault().employee.LName),
                    TotalSells = (double)e.Sum(t => (decimal)t.TotalPrice)
                })
                .OrderByDescending(e=>e.TotalSells).Take(3).ToList();
        }
        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(Transaction entity)
        {
            applicationDbContext.Transactions.Update(entity);
        }
    }
}
