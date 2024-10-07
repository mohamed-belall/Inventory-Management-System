﻿using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.Models
{
    public class Employee
    {
        
        public int ID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public DateTime CreatedDate { get; set; }
        



        public List<StartAlert>  startAlerts { get; set; }

        public List<Transaction> transactions { get; set; }

        public List<EmployeeSupplier> employeeSuppliers { get; set; }
    }
}