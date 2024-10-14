using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModel
{
    public class EmployeeSupplierWithSupplierList
    {
        public int Id { get; set; } // Primary key
        public int EmployeeID { get; set; }
        public List<Employee>? Employees { get; set; }
        public int SupplierID { get; set; }
        public List<Supplier>? Suppliers { get; set; }

        // Additional attributes
        public int ProductIdentifier { get; set; }
        public List<Product>? Products { get; set; }
        public DateTime StartDate { get; set; }
        public double TotalCost { get; set; }
        public int Quantity { get; set; }
    }
}
