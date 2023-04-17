using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;

namespace Generator.Pages.Encounters
{
    public class DeleteModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public DeleteModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Encounter Encounter { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Encounter == null)
            {
                return NotFound();
            }

            var encounter = await _context.Encounter.FirstOrDefaultAsync(m => m.EncounterId == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Encounter == null)
            {
                return NotFound();
            }
            var encounter = await _context.Encounter.FindAsync(id);

            if (encounter != null)
            {
                Encounter = encounter;
                _context.Encounter.Remove(Encounter);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
