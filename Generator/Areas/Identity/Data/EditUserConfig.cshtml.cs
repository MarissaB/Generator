using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Generator.Areas.Identity.Data
{
    public class EditUserConfigModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public EditUserConfigModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserConfig UserConfig { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.UserConfig == null)
            {
                return NotFound();
            }

            var userconfig =  await _context.UserConfig.FirstOrDefaultAsync(m => m.UserConfigId == id);
            if (userconfig == null)
            {
                return NotFound();
            }
            UserConfig = userconfig;
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

            _context.Attach(UserConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserConfigExists(UserConfig.UserConfigId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserConfigExists(int id)
        {
          return (_context.UserConfig?.Any(e => e.UserConfigId == id)).GetValueOrDefault();
        }
    }
}
