
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
            throw new NotImplementedException();
        }

        public void Delete(StartAlert entity)
        {
            throw new NotImplementedException();
        }

        public List<StartAlert> GetAll()
        {
            throw new NotImplementedException();
        }

        public StartAlert GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(StartAlert entity)
        {
            throw new NotImplementedException();
        }
    }
}
