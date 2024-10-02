
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
            throw new NotImplementedException();
        }

        public void Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> GetAll()
        {
            throw new NotImplementedException();
        }

        public Transaction GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
