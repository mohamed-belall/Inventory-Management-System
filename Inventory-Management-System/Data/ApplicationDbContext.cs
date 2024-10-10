using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSupplier> EmployeeSuppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StartAlert> StartAlerts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring composite key for EmployeeSupplier
            modelBuilder.Entity<EmployeeSupplier>()
                .HasKey(es => es.Id); // Id is now the primary key

            modelBuilder.Entity<EmployeeSupplier>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.employeeSuppliers) // Assuming Employee has a collection of EmployeeSuppliers
                .HasForeignKey(es => es.EmployeeID);

            modelBuilder.Entity<EmployeeSupplier>()
                .HasOne(es => es.Supplier)
                .WithMany(s => s.employeeSuppliers) // Assuming Supplier has a collection of EmployeeSuppliers
                .HasForeignKey(es => es.SupplierID);
        }
    }
}
