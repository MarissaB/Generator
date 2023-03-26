using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;
using Generator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Generator.Authorization;

namespace Generator.Pages.Vessels
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        [BindProperty]
      public Vessel Vessel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Vessel? vessel = await Context.Vessel.FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null)
            {
                return NotFound();
            }
            Vessel = vessel;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Vessel, Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var vessel = await Context.Vessel.AsNoTracking().FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Vessel, Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            Context.Vessel.Remove(Vessel);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool VesselExists(int id)
        {
            return (Context.Vessel?.Any(e => e.VesselId == id)).GetValueOrDefault();
        }
    }
}
