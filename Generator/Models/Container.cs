using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Container
    {
        public int ContainerId { get; set; }
        public string? Name { get; set; }
        [Display(Name = "Treasures")]
        [DefaultValue(1)]
        public int TreasureCapacity { get; set; }
        [DefaultValue(1)]
        public Size TreasureMaxSize { get; set; }
        [Display(Name = "Icon")]
        [DefaultValue("Default")]
        public string? Image { get; set; }
    }
}
