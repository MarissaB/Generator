using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generator.Data;

namespace Generator.Areas.Identity.Data
{
    public class CreateModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public CreateModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserConfig UserConfig { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.UserConfig == null || UserConfig == null)
            {
                return Page();
            }

            UserConfig.UserId = "placeholder";

            _context.UserConfig.Add(UserConfig);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
