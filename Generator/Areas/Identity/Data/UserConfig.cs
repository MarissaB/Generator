
using System.ComponentModel;

namespace Generator.Areas.Identity.Data
{
    public class UserConfig
    {
        public int UserConfigId { get; set; }
        public string UserId { get; set; }
        [DisplayName("Display Name")]
        public string? DisplayName { get; set; }
        [DisplayName("Theme")]
        public string? Theme { get; set; }
        [DisplayName("Avatar")]
        public string? Image { get; set; }
    }
}
