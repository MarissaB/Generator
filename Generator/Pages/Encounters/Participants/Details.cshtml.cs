using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;

namespace Generator.Pages.Encounters.Participants
{
    public class DetailsModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public DetailsModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Participant Participant { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Participant == null)
            {
                return NotFound();
            }

            var participant = await _context.Participant.FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }
            else 
            {
                Participant = participant;
            }
            return Page();
        }
    }
}
