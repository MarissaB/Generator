using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Treasure
    {
        public int TreasureId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "")]
        public string? Image { get; set; }
    }
}
