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

        public string LootMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Container? container = await _context.Container.FirstOrDefaultAsync(v => v.ContainerId == id);
            if (container == null)
            {
                return NotFound();
            }
            Container = container;
            if (!RollForMimic())
            {
                LootMessage = "You have found the following loot.";
                FillTreasures();
            }
            else
            {
                LootMessage = "It's empty. Maybe a mimic ate everything :(";
            }
            return Page();
        }
        /// <summary>
        /// Determines if a container has any loot at all (90%) or if it's empty from a mimic eating the treasure (10%).
        /// </summary>
        /// <returns>true if there's a mimic, i.e. no treasure in the container</returns>
        private static bool RollForMimic()
        {
            int roll = Random.Shared.Next(1, 100);
            return roll <= 10;
        }

        /// <summary>
        /// Determines if a container has a Legendary (1%), Epic (5%), or Rare (10%) item. Will always include Common and Uncommon.
        /// </summary>
        /// <returns>List of eligible Rarity types</returns>
        private static List<Rarity> RollForRarity()
        {
            var list = new List<Rarity>
            {
                Rarity.Common,
                Rarity.Uncommon
            };

            int roll = Random.Shared.Next(1, 100);
            if (roll == 100)                { list.Add(Rarity.Legendary); }
            if (roll >= 95 && roll < 100)   { list.Add(Rarity.Epic); }
            if (roll >= 85 && roll < 95)    { list.Add(Rarity.Rare); }

            return list;
        }

        /// <summary>
        /// Fill Container with Treasures based on rarity roll and capacity
        /// </summary>
        private void FillTreasures()
        {
            Treasures = new List<Treasure>();
            if (Container.TreasureCapacity > 0 && _context.Treasure != null)
            {
                // Roll to see what rarities of treasure the list will include
                List<Rarity> rarity = RollForRarity();

                // If there's a special rarity treasure, grab all eligible treasures that fit the container's size limit
                // and required rarity, then pick 1 to include in the container.
                if (rarity.Count > 2)
                {
                    List<Treasure> specialTreasures = _context.Treasure.Where(t => t.Size <= Container.TreasureMaxSize &&
                                                                           t.Rarity == rarity[2]).ToList();
                    int randomIndex = Random.Shared.Next(0, specialTreasures.Count);
                    Treasures.Add(specialTreasures[randomIndex]);
                    LootMessage += " This " + Container.Name + " contained a special " + rarity[2].ToString() + " item!";
                }

                // If the container still has room, grab all eligible Uncommon and Common treasures that fit the size limit,
                // and fill the container with them to complete the list.
                int remainingSpaces = Container.TreasureCapacity - Treasures.Count;
                if (remainingSpaces > 0)
                {
                    List<Treasure> mundaneTreasures = _context.Treasure.Where(t => t.Size <= Container.TreasureMaxSize &&
                                                                               (t.Rarity == Rarity.Common || t.Rarity == Rarity.Uncommon)).ToList();
                    for (int i = 0; i < remainingSpaces; i++)
                    {
                        int randomIndex = Random.Shared.Next(0, mundaneTreasures.Count);
                        Treasures.Add(mundaneTreasures[randomIndex]);
                    }
                }

                // Sort neatly because we're adventurers, not heathens!
                Treasures = Treasures.OrderBy(t => t.Name).ToList();
            }
        }
    }
}
