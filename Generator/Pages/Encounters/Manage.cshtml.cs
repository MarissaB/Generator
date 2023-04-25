using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Generator.Authorization;

namespace Generator.Pages.Encounters
{
    public class ManageModel : DI_BasePageModel
    {
        public ManageModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public IList<Encounter> Encounter { get;set; } = default!;

        public async Task OnGetAsync()
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
            }
        }

        public async Task<IActionResult> OnPostDeleteEncounter(int encounterId)
        {
            if (Context.Encounter == null)
            {
                return NotFound();
            }
            var encounter = await Context.Encounter
                        .Where(e => e.EncounterId == encounterId)
                        .Include(e => e.Participants).FirstAsync();

            if (encounter != null)
            {
                Encounter deleteEncounter = encounter;
                Context.Encounter.Remove(deleteEncounter);
                if (deleteEncounter.Participants != null)
                {
                    Context.Participant.RemoveRange(deleteEncounter.Participants);
                }
                await Context.SaveChangesAsync();
            }
            return RedirectToPage("./Manage");
        }
    }
}
