using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Generator.Authorization;

namespace Generator.Pages.Encounters
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        [BindProperty]
        public Encounter Encounter { get; set; } = default!;
        [BindProperty]
        public IList<Participant> Participants { get; set; }

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



        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
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

            return RedirectToPage("./Manage");
        }

        public async Task<IActionResult> OnPostDeleteParticipant(int participantId)
        {
            Participant foundParticipant = new Participant();
            if (Context.Participant == null)
            {
                return NotFound();
            }
            var participant = await Context.Participant.FindAsync(participantId);

            if (participant != null)
            {
                foundParticipant = participant;
                Context.Participant.Remove(foundParticipant);
                await Context.SaveChangesAsync();
            }
            return RedirectToPage("./Edit", new { id = foundParticipant.EncounterId });
        }

        private bool EncounterExists(int id)
        {
          return (Context.Encounter?.Any(e => e.EncounterId == id)).GetValueOrDefault();
        }
    }
}
