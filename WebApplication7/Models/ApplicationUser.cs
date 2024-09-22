using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models
{
    public class ApplicationUser: IdentityUser
    {
        [RegularExpression(@"^[a-zA-Z0-9_]{1,10}$", ErrorMessage = "DomainName can only be 10 characters, alphanumeric, or underscore.")]
        public string? DomainName { get; set; }
    }
}
