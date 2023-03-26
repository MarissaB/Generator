using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;

namespace Generator.Pages.Creatures
{
    public class IndexModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public IndexModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Creature> Creature { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Creature != null)
            {
                Creature = await _context.Creature.ToListAsync();
            }
        }
    }
}
