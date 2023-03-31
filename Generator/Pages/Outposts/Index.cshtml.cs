﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Models;
using Microsoft.AspNetCore.Authorization;

namespace Generator.Pages.Outposts
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public IndexModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Outpost> Outpost { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Outpost != null)
            {
                Outpost = await _context.Outpost.ToListAsync();
            }
        }
    }
}