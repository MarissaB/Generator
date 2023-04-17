using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;

namespace Generator.Pages.Encounters
{
    public class DetailsModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public DetailsModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Encounter Encounter { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Encounter == null)
            {
                return NotFound();
            }

            var encounter = await _context.Encounter.Include(e => e.Participants).FirstOrDefaultAsync(m => m.EncounterId == id);
            if (encounter == null)
            {
                return NotFound();
            }
            else 
            {
                Encounter = encounter;
            }
            return Page();
        }
    }
}
