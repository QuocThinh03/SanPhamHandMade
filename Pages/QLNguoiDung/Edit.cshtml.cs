using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLNguoiDung
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(NguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguoiDungExists(NguoiDung.ID))
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

        private bool NguoiDungExists(int? id)
        {
            return _context.NguoiDung.Any(e => e.ID == id);
        }
    }
}
