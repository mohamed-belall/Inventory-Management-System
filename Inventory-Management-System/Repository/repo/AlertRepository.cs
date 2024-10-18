
using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
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
        public List<int>? GetAllAlertedProductsIds()
        {
            return applicationDbContext.StartAlerts.Where(a => a.IsResolved == false).Select(a=>a.ProductId).ToList();
        }
        public List<StartAlert> GetAlertWithAllData()
        {
            return applicationDbContext.StartAlerts
                .AsNoTracking()
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




        public List<StartAlert> GetFilterByName(string name)
        {

            if (name == string.Empty || name == null)
                return GetAlertWithAllData();
            else
               return applicationDbContext.StartAlerts
                    .Include(a => a.product)
                    .Include(a => a.employee)
                    .Where(p => p.product.Name.Contains(name))
                    .ToList();
        }


        public List<StartAlert> GetStatus(string status)
        {
            if(status == "Pending")
            {
                return applicationDbContext.StartAlerts
                 .Include(a => a.product)
                 .Include(a => a.employee)
                 .Where(p => p.IsResolved == false)
                 .ToList();


            }
            else if(status == "Completed")
            {
                return applicationDbContext.StartAlerts
                 .Include(a => a.product)
                 .Include(a => a.employee)
                 .Where(p => p.IsResolved == true)
                 .ToList();
            }
            return GetAlertWithAllData();
        }

        public List<StartAlert> GetSearchAndStatusResult(string name, string status)
        {
            if (name == string.Empty || name == null)
            {
                if (status == "Pending")
                {
                    return applicationDbContext.StartAlerts
                        .AsNoTracking()
                     .Include(a => a.product)
                     .Include(a => a.employee)
                     .Where(p => p.IsResolved == false)
                     .ToList();


                }
                else if (status == "Completed")
                {
                    return applicationDbContext.StartAlerts
                        .AsNoTracking()
                     .Include(a => a.product)
                     .Include(a => a.employee)
                     .Where(p => p.IsResolved == true)
                     .ToList();
                }
                return GetAlertWithAllData();

            }
            else
            {
                if (status == "Pending")
                {
                    return applicationDbContext.StartAlerts
                        .AsNoTracking()
                     .Include(a => a.product)
                     .Include(a => a.employee)
                     .Where(p => p.IsResolved == false && p.product!.Name.Contains(name))
                     .ToList();


                }
                else if (status == "Completed")
                {
                    return applicationDbContext.StartAlerts
                        .AsNoTracking()
                     .Include(a => a.product)
                     .Include(a => a.employee)
                     .Where(p => p.IsResolved == true && p.product!.Name.Contains(name))
                     .ToList();
                }
                else
                {
                    return applicationDbContext.StartAlerts
                        .AsNoTracking()
                         .Include(a => a.product)
                         .Include(a => a.employee)
                         .Where(p => p.product!.Name.Contains(name))
                         .ToList();
                }


            }
        }

        public StartAlert GetByProductId(int id)
        {
            return applicationDbContext.StartAlerts.FirstOrDefault(a => a.ProductId == id);
        }



        public List<StartAlert> GetPendingAlert()
        {
            return applicationDbContext.StartAlerts
                .AsNoTracking()
                .Include(a => a.product)
                .Where(a => a.IsResolved == false).ToList();
        }

        public List<StartAlert> GetPagedAlert(int page, int pageSize)
        {
            return applicationDbContext.StartAlerts
                            .AsNoTracking()
                            .Include(a => a.product)
                            .Include(a => a.employee)
                            .OrderBy(s => s.ID)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
        }
    }
}
