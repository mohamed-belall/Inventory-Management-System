namespace Inventory_Management_System.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public int GetLastTransactionId();
        public List<TopEmployee> GetTopEmployees();

    }
}
