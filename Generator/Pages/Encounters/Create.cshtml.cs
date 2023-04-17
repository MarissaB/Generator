using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Generator.Models;
using Microsoft.AspNetCore.Identity;

namespace Generator.Pages.Encounters
{
    public class CreateModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(Generator.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Encounter = new Encounter
            {
                UserId = user.Id,
                TurnIndex = 0,
                Round = 1
            };
            return Page();
        }

        [BindProperty]
        public Encounter Encounter { get; set; } = default!;
        public IEnumerable<Participant> Participants { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Encounter == null || Encounter == null)
            {
                return Page();
            }
            _context.Encounter.Add(Encounter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./CreateParticipant", new { id = Encounter.EncounterId });
        }
    }
}
