namespace Inventory_Management_System.Repository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        public int GetAllCount();

    }
}
