using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;
using Generator.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Generator.Pages.Encounters
{
    // TODO: Set up display for Participants in the same page by selection. See https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-7.0
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public IList<Encounter> Encounter { get;set; } = default!;
        public int EncounterId { get; set; }
        public IList<Participant> Participants { get;set; }

        public async Task OnGetAsync(int? id)
        {
            var user = await UserManager.GetUserAsync(User);
            if (Context.Encounter != null && user != null)
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, null, Operations.ReadAll);
                if (isAuthorized.Succeeded)
                {
                    Encounter = await Context.Encounter
                        .Include(e => e.Participants)
                        .ToListAsync();
                }
                else
                {
                    Encounter = await Context.Encounter
                        .Where(e => e.UserId == user.Id)
                        .Include(e => e.Participants).ToListAsync();
                }

                if (id != null) // If an Encounter is selected, fetch its Participants.
                {
                    EncounterId = id.Value; // Enables the selected row highlighting in html on value match
                    Encounter selectedEncounter = Encounter.Where(e => e.EncounterId == id.Value).FirstOrDefault();
                    Participants = selectedEncounter.Participants.ToList();
                }
            }
        }
    }
}
