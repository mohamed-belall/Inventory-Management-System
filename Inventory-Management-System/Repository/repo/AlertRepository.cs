
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repository.repo
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AlertRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(StartAlert entity)
        {
            applicationDbContext.StartAlerts.Add(entity);
        }

        public void Delete(StartAlert entity)
        {
            applicationDbContext.StartAlerts.Remove(entity);
        }

        public List<StartAlert> GetAll()
        {
           return applicationDbContext.StartAlerts.ToList();
        }

        public List<StartAlert> GetAlertWithAllData()
        {
            return applicationDbContext.StartAlerts
                .Include(a => a.employee)
                .Include(a => a.product)
                .ToList();
        }

        public StartAlert GetById(int id)
        {
           return applicationDbContext.StartAlerts.FirstOrDefault(a => a.ID == id)!;
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(StartAlert entity)
        {
            applicationDbContext.StartAlerts.Update(entity);
        }
    }
}
