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
    public class DetailsModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public DetailsModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
