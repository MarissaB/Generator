using Generator.Authorization;
using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Generator.Pages.Vessels
{
    [AllowAnonymous]
    public class FillModel : PageModel
    {
        // TODO: After loading the Vessel, display a number of filler elements to match Treasure capacities
        // TODO: Implement random lookup for which Treasures to display
        // TODO: Implement random lookup for which Creatures to display
        private readonly ApplicationDbContext _context;
        public FillModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Vessel Vessel { get; set; } = default!;
        public IList<Creature> Creatures { get; set; } = default!;
        public IList<Treasure> Treasures { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Vessel? vessel = await _context.Vessel.FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null)
            {
                return NotFound();
            }
            Vessel = vessel;
            FillCreatures();
            FillTreasures();
            return Page();
        }

        private void FillCreatures()
        {
            Creatures = new List<Creature>();
            if (Vessel.CreatureCapacity > 0 && _context.Creature != null)
            {
                Creatures = _context.Creature.Take(Vessel.CreatureCapacity).ToList();
            }
        }

        private void FillTreasures()
        {
            Treasures = new List<Treasure>();
            if (Vessel.TreasureCapacity > 0 && _context.Treasure != null)
            {
                Treasures = _context.Treasure.Take(Vessel.TreasureCapacity).ToList();
            }
        }
    }
}
