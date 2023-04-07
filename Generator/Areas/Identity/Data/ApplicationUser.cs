using Microsoft.AspNetCore.Identity;

namespace Generator.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string DisplayName { get; set; }
    }
}
