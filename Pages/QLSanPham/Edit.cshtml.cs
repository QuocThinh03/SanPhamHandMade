using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLSanPham
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class EditModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;
        public List<Loai> ListLoai { get; set; }
        public EditModel(QuocThinh.Models.ApplicationDbContext context)
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

            ListLoai = await _context.Loai.ToListAsync() ?? new List<Loai>();

            return Page();
        }

        [BindProperty]
        public IFormFile Anh { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Anh != null && Anh.Length > 0)
            {
                var fileName = Path.GetFileName(Anh.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Client","img","sanpham", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Anh.CopyToAsync(stream);
                }

                SanPham.Anh = fileName;
            }

            _context.Attach(SanPham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(SanPham.ID))
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
        private bool SanPhamExists(int? id)
        {
            return _context.SanPham.Any(e => e.ID == id);
        }
    }
}
