using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Microsoft.AspNetCore.Identity;

namespace Generator.Areas.Identity.Data
{
    // TODO: Update to Bootstrap 5.3 when it comes out since floating form labels are overlapping with inputs. https://github.com/twbs/bootstrap/issues/37539
    public class EditModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EditModel(Generator.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public UserConfig UserConfig { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.UserConfig == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserConfig = new UserConfig { UserId = user.Id };
            var userconfig = await _context.UserConfig.FirstOrDefaultAsync(m => m.UserId == user.Id);
            if (userconfig != null)
            {
                UserConfig = userconfig;
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserConfig.UserId = user.Id;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userconfig = await _context.UserConfig.AsNoTracking().FirstOrDefaultAsync(m => m.UserId == user.Id);
                if (userconfig == null)
                {
                    _context.UserConfig.Add(UserConfig);
                }
                else
                {
                    _context.Attach(UserConfig).State = EntityState.Modified;
                }
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

            return RedirectToPage();
        }

        private bool UserConfigExists(int id)
        {
          return (_context.UserConfig?.Any(e => e.UserConfigId == id)).GetValueOrDefault();
        }
    }
}
