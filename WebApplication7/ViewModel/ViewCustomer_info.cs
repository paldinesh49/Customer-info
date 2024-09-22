using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModel
{
    public class ViewCustomer_info
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public byte GenderId { get; set; }

        [Required]
        public int StateId { get; set; }
        [Required]
        public int DistrictId { get; set; }
    }
}
