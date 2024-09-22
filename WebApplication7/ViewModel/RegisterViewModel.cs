using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]{1,10}$", ErrorMessage = "DomainName can only be 10 characters, alphanumeric, or underscore.")]
        public string? DomainName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,}$",
           ErrorMessage = "Password must be at least 6 characters long and contain at least one digit, one lowercase letter, and one uppercase letter.")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

