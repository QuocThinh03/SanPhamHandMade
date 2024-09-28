using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLLoai
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class DeleteModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public DeleteModel(QuocThinh.Models.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Loai = await _context.Loai.FindAsync(id);

            if (Loai != null)
            {
                _context.Loai.Remove(Loai);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
