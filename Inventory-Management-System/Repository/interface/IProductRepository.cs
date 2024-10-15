namespace Inventory_Management_System.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        public List<Product> GetFilteredByCategory(int? id);
        public List<Product> GetFilteredByName(string name);
        public List<Product> GetFilteredByNameWithCategory(string name, int? id);
        public List<Product> GetFilteredByStatus(string staus, int? id);

        public int GetProductCount();
        public int GetItemsCount();
    }

}


