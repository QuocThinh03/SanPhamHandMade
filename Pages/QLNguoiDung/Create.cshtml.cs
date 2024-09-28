using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLNguoiDung
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class CreateModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public CreateModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NguoiDung NguoiDung { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.NguoiDung.Add(NguoiDung);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
