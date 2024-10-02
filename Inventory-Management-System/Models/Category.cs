namespace Inventory_Management_System.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public List<Product> products { get; set; }
    }
}
