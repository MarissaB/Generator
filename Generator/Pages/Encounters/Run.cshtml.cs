using Generator.Authorization;
using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Generator.Pages.Encounters
{
    public class RunModel : DI_BasePageModel
    {
        // TODO: Update display on Participants to be something better than a table
        // TODO: Add generic Post for updating Participant
            // TODO: Add function to display Participant RemainingHealth
            // TODO: Add functions to increase (damage) and decrease (heal) Participant RemovedHealth
            // TODO: Add function to edit Participant Note
            // TODO: Add switch to toggle Participant IsActive along with UI updates to denote inactives in gray
        public RunModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        [BindProperty]
        public Encounter Encounter { get; set; }
        [BindProperty]
        public IList<Participant> Participants { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await UserManager.GetUserAsync(User);
            var encounter = new Encounter();
            if (Context.Encounter != null && user != null && id != null)
            {
                encounter = await Context.Encounter
                        .Include(e => e.Participants).FirstOrDefaultAsync(e => e.EncounterId == id && e.UserId == user.Id);
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
            if (Encounter.Round == null)
            {
                Encounter.Round = 1;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostNextRound()
        {
            if (Encounter.Round == null)
            {
                Encounter.Round = 1;
            }
            Encounter.Round++;
            return await OnPostAsync();
        }

        public async Task<IActionResult> OnPostPreviousRound()
        {
            if (Encounter.Round == null || Encounter.Round == 1)
            {
                Encounter.Round = 1;
            }
            else { Encounter.Round--; }
            return await OnPostAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user != null)
            {
                if (Encounter.UserId != user.Id)
                {
                    return Forbid();
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Encounter).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EncounterExists(Encounter.EncounterId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Run", new { id = Encounter.EncounterId });
        }

        private bool EncounterExists(int id)
        {
            return (Context.Encounter?.Any(e => e.EncounterId == id)).GetValueOrDefault();
        }
    }
}
