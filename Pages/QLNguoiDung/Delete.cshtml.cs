using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLNguoiDung
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
        public NguoiDung NguoiDung { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(m => m.ID == id);

            if (NguoiDung == null)
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

            NguoiDung = await _context.NguoiDung.FindAsync(id);

            if (NguoiDung != null)
            {
                _context.NguoiDung.Remove(NguoiDung);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
