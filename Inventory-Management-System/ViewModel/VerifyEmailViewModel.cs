using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System.ViewModel
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
