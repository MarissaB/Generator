using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Generator.Data;
using Generator.Models;

namespace Generator.Pages.Encounters.Participants
{
    public class EditModel : PageModel
    {
        private readonly Generator.Data.ApplicationDbContext _context;

        public EditModel(Generator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Participant Participant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Participant == null)
            {
                return NotFound();
            }

            var participant =  await _context.Participant.FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }
            Participant = participant;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(Participant.ParticipantId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Encounters/Index", new { id = Participant.EncounterId });
        }

        private bool ParticipantExists(int id)
        {
          return (_context.Participant?.Any(e => e.ParticipantId == id)).GetValueOrDefault();
        }
    }
}
