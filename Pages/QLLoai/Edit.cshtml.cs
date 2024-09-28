using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLLoai
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class EditModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public EditModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Loai Loai { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Loai = await _context.Loai.FirstOrDefaultAsync(m => m.ID == id);

            if (Loai == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Loai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiExists(Loai.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LoaiExists(int? id)
        {
            return _context.Loai.Any(e => e.ID == id);
        }
    }
}
