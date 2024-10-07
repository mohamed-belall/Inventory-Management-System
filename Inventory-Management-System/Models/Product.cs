﻿namespace Inventory_Management_System.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public string? Description { get; set; }


       
        public StartAlert startAlert { get; set; }


        public List<Transaction> transactions { get; set; }

        public int CategoryId { get; set; }
        public Category category { get; set; }


        public int SupplierId { get; set; }
        public Supplier supplier { get; set; }
    }
}