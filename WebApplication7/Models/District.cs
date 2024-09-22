using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public class District
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int StateId { get; set; } // Foreign Key for State

        [ForeignKey("StateId")]
        public virtual State? State { get; set; } // Navigation property for State

       
    }

}
