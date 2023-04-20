using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;
using Generator.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Generator.Pages.Encounters
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public IList<Encounter> Encounter { get;set; } = default!;
        public int EncounterId { get; set; }
        public string EncounterName { get; set; }
        public IList<Participant> Participants { get;set; }
        public Participant Participant { get; set; }

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
                    Encounter selectedEncounter = Encounter.FirstOrDefault(e => e.EncounterId == id.Value && e.UserId == user.Id);
                    if (selectedEncounter != null)
                    {
                        EncounterId = id.Value; // Enables the selected row highlighting in html on value match
                        EncounterName = selectedEncounter.Name;
                        if (selectedEncounter.Participants != null)
                        {
                            Participants = selectedEncounter.Participants.ToList();
                        }
                    }                    
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteParticipant(int participantId)
        {
            if (Context.Participant == null)
            {
                return NotFound();
            }
            var participant = await Context.Participant.FindAsync(participantId);

            if (participant != null)
            {
                Participant = participant;
                Context.Participant.Remove(Participant);
                await Context.SaveChangesAsync();
            }
            return RedirectToPage("./Index", new { id = Participant.EncounterId });
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
            return RedirectToPage("./Index");
        }

    }
}
