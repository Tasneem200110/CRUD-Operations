using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class ForgetPasswordVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
