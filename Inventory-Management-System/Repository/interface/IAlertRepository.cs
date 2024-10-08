namespace Inventory_Management_System.Repository
{
    public interface IAlertRepository:IRepository<StartAlert>
    {
        public List<StartAlert> GetAlertWithAllData();
    }
}
