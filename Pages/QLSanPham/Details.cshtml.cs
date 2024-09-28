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
    public class DetailsModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public DetailsModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
