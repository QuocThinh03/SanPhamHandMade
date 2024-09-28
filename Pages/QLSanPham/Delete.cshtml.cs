using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLSanPham
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
        public SanPham SanPham { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SanPham = await _context.SanPham.FirstOrDefaultAsync(m => m.ID == id);

            if (SanPham == null)
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

            SanPham = await _context.SanPham.FindAsync(id);

            if (SanPham != null)
            {
                _context.SanPham.Remove(SanPham);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
