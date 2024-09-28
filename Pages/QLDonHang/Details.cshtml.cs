using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuocThinh.Models;

namespace QuocThinh.Pages.QLDonHang
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class DetailsModel : PageModel
    {
        private readonly QuocThinh.Models.ApplicationDbContext _context;

        public DetailsModel(QuocThinh.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public DonHang DonHang { get; set; } = default!;
        public List<ChiTietDonHang> listchitietdonhang { get; set; } = new List<ChiTietDonHang>();
        public List<SanPham> ListSanPham { get; set; } = new List<SanPham>();
        public decimal TongGia { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.DonHang == null)
            {
                return NotFound();
            }

            var donhang = await _context.DonHang.FirstOrDefaultAsync(m => m.ID == id);

            listchitietdonhang = await _context.ChiTietDonHang.Where(ct => ct.IDDonHang == id).ToListAsync();
            TongGia = 0;

            foreach (var chitiet in listchitietdonhang)
            {
                var sanpham = await _context.SanPham.FirstOrDefaultAsync(sp => sp.ID == chitiet.IDSanPham);
                ListSanPham.Add(sanpham);

                TongGia = chitiet.SoLuong * chitiet.Gia;
            }
            if (donhang == null)
            {
                return NotFound();
            }
            else
            {
                DonHang = donhang;
            }
            return Page();
        }
    }
}
