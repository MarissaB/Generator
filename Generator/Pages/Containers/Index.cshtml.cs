using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;

namespace Generator.Pages.Containers
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public IndexModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Container> Container { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Container != null)
            {
                Container = await _context.Container.ToListAsync();
            }
        }
    }
}
