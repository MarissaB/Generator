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
        /// <summary>
        /// Generate random index values, then grab the Creatures at those indexes to fill the vessel
        /// </summary>
        private void FillCreatures()
        {
            Creatures = new List<Creature>();
            if (Vessel.CreatureCapacity > 0 && _context.Creature != null)
            {
                List<Creature> creaturesAvailable = _context.Creature.ToList();
                for (int i = 0; i < Vessel.CreatureCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, creaturesAvailable.Count);
                    Creatures.Add(creaturesAvailable[randomIndex]);
                }
                Creatures = Creatures.OrderBy(t => t.Name).ToList();
            }
        }

        /// <summary>
        /// Generate random index values, then grab the Treasures at those indexes to fill the vessel
        /// </summary>
        private void FillTreasures()
        {
            Treasures = new List<Treasure>();
            if (Vessel.TreasureCapacity > 0 && _context.Treasure != null)
            {
                List<Treasure> treasuresAvailable = _context.Treasure.ToList();
                for (int i = 0; i < Vessel.TreasureCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, treasuresAvailable.Count);
                    Treasures.Add(treasuresAvailable[randomIndex]);
                }
                Treasures = Treasures.OrderBy(t => t.Name).ToList();
            }


        }
    }
}
