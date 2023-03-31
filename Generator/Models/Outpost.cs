using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Outpost
    {
        public int OutpostId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Religious Sites")]
        public int? ReligionCapacity { get; set; }
        [Display(Name = "Artisans")]
        public int? ArtisanCapacity { get; set; }
        [Display(Name = "Specialty Shops")]
        public int? SpecialtyShopCapacity { get; set; }
    }

    public class ReligiousSite
    {
        public int ReligiousSiteId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }

    public class Artisan
    {
        public int ArtisanId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }

    public class SpecialtyShop
    {
        public int SpecialtyShopId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }
}
