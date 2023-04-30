using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generator.Data;
using Generator.Models;
using Generator.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Generator.Pages.Encounters.Participants
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }
        public List<Encounter> Encounters { get; set; }
        public SelectList EncounterNameSelection { get; set; }
        public int EncounterId { get; set; }

        public void PopulateEncounterDropdownList(object? selectedEncounter = null)
        {
            EncounterNameSelection = new SelectList(Encounters, nameof(Encounter.EncounterId), nameof(Encounter.Name), selectedEncounter);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await UserManager.GetUserAsync(User);
            if (Context.Encounter != null && user != null)
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, null, Operations.ReadAll);
                if (isAuthorized.Succeeded)
                {
                    Encounters = await Context.Encounter.ToListAsync();
                }
                else
                {
                    Encounters = await Context.Encounter.Where(x => x.UserId == user.Id).ToListAsync();
                }
            }
            Encounter emptyEncounter = new Encounter();
            if (id != null)
            {
                emptyEncounter.EncounterId = id.Value;
                EncounterId = id.Value;
            }
            PopulateEncounterDropdownList(emptyEncounter.EncounterId);
            return Page();
        }

        [BindProperty]
        public Participant Participant { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (Context.Encounter != null)
            {
                Encounter encounter = Context.Encounter.FirstOrDefault(e => e.EncounterId == Participant.EncounterId);
                Participant.Encounter = encounter;
            }
            if (!ModelState.IsValid || Context.Participant == null || Participant == null)
            {
                return Page();
            }

            Context.Participant.Add(Participant);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Encounters/Edit", new { id = Participant.EncounterId });
        }
    }
}
