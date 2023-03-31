using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Generator.Pages.Vessels
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public IList<Vessel> Vessel { get; set; }

        public async Task OnGetAsync()
        {
            var vessels = from v in Context.Vessel select v;
            Vessel = await vessels.ToListAsync();
        }
    }
}
