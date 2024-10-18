namespace Inventory_Management_System.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public int GetLastTransactionId();
        public List<TopEmployee> GetTopEmployees();
        public int GetTrabsactionsCount();
        public double GetTotalSells();
        public List<SalesHistory> GetHistorySalesDictionary();
        public List<Transaction> GetPagedtransaction(int page, int pageSize);


    }
}
