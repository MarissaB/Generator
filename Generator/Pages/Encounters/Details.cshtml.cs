using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;
using Generator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Generator.Authorization;

namespace Generator.Pages.Encounters
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public Encounter Encounter { get; set; } = default!;
        public IList<Participant>? Participants { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await UserManager.GetUserAsync(User);
            var encounter = new Encounter();
            if (Context.Encounter != null && user != null && id != null)
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, null, Operations.ReadAll);
                if (isAuthorized.Succeeded)
                {
                    encounter = await Context.Encounter
                        .Include(e => e.Participants).FirstOrDefaultAsync(e => e.EncounterId == id);
                }
                else
                {
                    encounter = await Context.Encounter
                        .Include(e => e.Participants).FirstOrDefaultAsync(e => e.EncounterId == id && e.UserId == user.Id);
                }
            }
            if (encounter == null)
            {
                return NotFound();
            }
            Encounter = encounter;
            if (Encounter.Participants != null)
            {
                Participants = Encounter.Participants.ToList();
            }
            return Page();
        }
    }
}
