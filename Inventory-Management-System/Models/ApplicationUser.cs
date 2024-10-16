using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Inventory_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? Employee_id { get; set; }
        public string? FName { get; set; }

        public string? LName { get; set; }
    }
}
