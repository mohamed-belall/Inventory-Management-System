namespace Inventory_Management_System.Models
{
    public class EmployeeSupplier
    {


        public int EmployeeID { get; set; }
        public virtual Employee employee { get; set; }

        public int SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }


        // Attribute of the relationship
        public DateTime StartDate { get; set; }
        public Double TotalCost { get; set; }
        public int Quantity { get; set; }
    }
}
