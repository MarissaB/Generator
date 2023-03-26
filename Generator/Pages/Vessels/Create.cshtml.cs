using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Generator.Models;
using Generator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Generator.Authorization;

namespace Generator.Pages.Vessels
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Vessel Vessel { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Vessel, Operations.Create);
            Context.Vessel.Add(Vessel);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
