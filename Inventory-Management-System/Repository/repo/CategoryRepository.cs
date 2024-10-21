
namespace Inventory_Management_System.Repository.repo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public void Add(Category entity)
        {
            applicationDbContext.Add(entity);
        }

        public void Delete(Category entity)
        {
            applicationDbContext.Remove(entity);
        }

        public List<Category> GetAll()
        {
            return applicationDbContext.Categories.ToList();
        }
        public int GetAllCount()
        {
            return applicationDbContext.Categories.Count();
        }
        public Category GetById(int id)
        {
            return applicationDbContext.Categories.FirstOrDefault(e => e.ID == id);
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }

        public void Update(Category entity)
        {
            applicationDbContext.Update(entity);
        }
    }
}
