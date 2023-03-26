using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;
using Generator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Generator.Pages.Vessels
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Vessel Vessel { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Vessel? vessel = await Context.Vessel.FirstOrDefaultAsync(v=>v.VesselId == id);
            if (vessel == null)
            {
                return NotFound();
            }
            Vessel = vessel;
            return Page();
        }
    }
}
