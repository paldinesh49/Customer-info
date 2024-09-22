using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models
{
    public class State
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<District> Districts { get; set; } = new List<District>();
    }

}
