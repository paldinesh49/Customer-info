using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication7.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        // DbSets (tables)
        public DbSet<Customer_Info> Customer_Info { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }

       


    }
}
