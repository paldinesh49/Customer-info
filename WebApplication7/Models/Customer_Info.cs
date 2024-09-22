using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public class Customer_Info
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public byte GenderId { get; set; }

        [Required]
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public virtual State? State { get; set; }  // Navigation property for State

        [Required]
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District? District { get; set; }  // Navigation property for District
    }

}
