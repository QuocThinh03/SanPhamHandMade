using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuocThinh.Models;
using Microsoft.EntityFrameworkCore;

namespace QuocThinh.Pages.QLSanPham
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class CreateModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;
        public List<Loai> ListLoai { get; set; }
        public CreateModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            ListLoai = await _context.Loai.ToListAsync() ?? new List<Loai>();
            return Page();
        }

        [BindProperty]
        public SanPham SanPham { get; set; }


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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/img/sanpham", fileName);
                                
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Anh.CopyToAsync(stream);
                }

                SanPham.Anh = fileName;
            }

            _context.SanPham.Add(SanPham);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}