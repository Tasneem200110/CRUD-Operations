using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class ResetPasswordVM
    {
        [Required]
        [MinLength(6, ErrorMessage = "Minimum Length For Password is 6 characters")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords isn't Matches")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
