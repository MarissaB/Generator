using Generator.Data;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Generator.Pages.Outposts
{
    [AllowAnonymous]
    public class FillModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public FillModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Outpost Outpost { get; set; } = default!;
        public IList<ReligiousSite> ReligiousSites { get; set; } = default!;
        public IList<Artisan> Artisans { get; set; } = default!;
        public IList<SpecialtyShop> SpecialtyShops { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Outpost? outpost = await _context.Outpost.FirstOrDefaultAsync(v => v.OutpostId == id);
            if (outpost == null)
            {
                return NotFound();
            }
            Outpost = outpost;
            FillReligiousSites();
            FillArtisans();
            FillSpecialtyShops();
            return Page();
        }

        private void FillSpecialtyShops()
        {
            SpecialtyShops = new List<SpecialtyShop>();
            if (Outpost.SpecialtyShopCapacity > 0 && _context.SpecialtyShop != null)
            {
                List<SpecialtyShop> available = _context.SpecialtyShop.ToList();
                for (int i = 0; i < Outpost.SpecialtyShopCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, available.Count);
                    SpecialtyShops.Add(available[randomIndex]);
                }
                SpecialtyShops = SpecialtyShops.OrderBy(t => t.Name).ToList();
            }
        }

        private void FillArtisans()
        {
            Artisans = new List<Artisan>();
            if (Outpost.ArtisanCapacity > 0 && _context.Artisan != null)
            {
                List<Artisan> available = _context.Artisan.ToList();
                for (int i = 0; i < Outpost.ArtisanCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, available.Count);
                    Artisans.Add(available[randomIndex]);
                }
                Artisans = Artisans.OrderBy(t => t.Name).ToList();
            }
        }

        private void FillReligiousSites()
        {
            ReligiousSites = new List<ReligiousSite>();
            if (Outpost.ReligionCapacity > 0 && _context.ReligiousSite != null)
            {
                List<ReligiousSite> available = _context.ReligiousSite.ToList();
                for (int i = 0; i < Outpost.ReligionCapacity; i++)
                {
                    int randomIndex = Random.Shared.Next(0, available.Count);
                    ReligiousSites.Add(available[randomIndex]);
                }
                ReligiousSites = ReligiousSites.OrderBy(t => t.Name).ToList();
            }
        }
    }
}
