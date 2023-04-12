using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Data;

namespace Generator.Areas.Identity.Data
{
    public class DeleteModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public DeleteModel(Generator.Data.ApplicationDbContext context)
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

            var userconfig = await _context.UserConfig.FirstOrDefaultAsync(m => m.UserConfigId == id);

            if (userconfig == null)
            {
                return NotFound();
            }
            else 
            {
                UserConfig = userconfig;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.UserConfig == null)
            {
                return NotFound();
            }
            var userconfig = await _context.UserConfig.FindAsync(id);

            if (userconfig != null)
            {
                UserConfig = userconfig;
                _context.UserConfig.Remove(UserConfig);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
