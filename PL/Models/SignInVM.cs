using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class SignInVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Minimum Length For Password is 6 characters")]
        public string Password { get; set; }

        public bool Remember_Me { get; set; }
    }
}
