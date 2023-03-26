using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Vessel
    {
        public int VesselId { get; set; }
        public string? Name { get; set; }
        [Display(Name = "Creatures")]
        public int CreatureCapacity { get; set; }
        [Display(Name = "Treasures")]
        public int TreasureCapacity { get; set; }
        [Display(Name = "")]
        public string? Image { get; set; }
    }
}
