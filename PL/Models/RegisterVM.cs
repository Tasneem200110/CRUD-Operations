using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Minimum Length For Password is 6 characters")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords isn't Matches")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool IsAgree { get; set; }

    }
}
