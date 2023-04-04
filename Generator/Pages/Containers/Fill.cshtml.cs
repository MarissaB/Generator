using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Generator.Pages.Containers
{
    [AllowAnonymous]
    public class FillModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public FillModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Container Container { get; set; } = default!;
        public IList<Treasure> Treasures { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Container? container = await _context.Container.FirstOrDefaultAsync(v => v.ContainerId == id);
            if (container == null)
            {
                return NotFound();
            }
            Container = container;
            FillTreasures();
            return Page();
        }

        /// <summary>
        /// Generate random index values, then grab the Treasures at those indexes to fill the container
        /// </summary>
        private void FillTreasures()
        {
            Treasures = new List<Treasure>();
            if (Container.TreasureCapacity > 0 && _context.Treasure != null)
            {
                List<Treasure> treasuresAvailable = _context.Treasure.Where(t => t.Size <= Container.TreasureMaxSize).ToList();
                for (int i = 0; i < Container.TreasureCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, treasuresAvailable.Count);
                    Treasures.Add(treasuresAvailable[randomIndex]);
                }
                Treasures = Treasures.OrderBy(t => t.Name).ToList();
            }
        }
    }
}
