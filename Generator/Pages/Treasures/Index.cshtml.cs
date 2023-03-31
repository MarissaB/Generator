using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;

namespace Generator.Pages.Treasures
{
    public class IndexModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public IndexModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Treasure> Treasure { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Treasure != null)
            {
                Treasure = await _context.Treasure.ToListAsync();
            }
        }
    }
}
