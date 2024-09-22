using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,}$",
           ErrorMessage = "Invalid password")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
