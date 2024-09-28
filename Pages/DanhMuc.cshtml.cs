using QuocThinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuocThinh.Pages
{
    public class DanhMucModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DanhMucModel> _logger;

        public List<SanPham> ListSanPham { get; set; }
        public List<Loai> ListLoai { get; set; }
        public string NavCssClass { get; set; }

        public DanhMucModel(ILogger<DanhMucModel> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            NavCssClass = "category-style-2";
            try
            {
                ListLoai = await _dbContext.Loai.ToListAsync();
                ListSanPham = await _dbContext.SanPham.Where(sp => sp.IDLoai == id).ToListAsync();

                if (ListSanPham == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching data for DanhMuc page");
                throw; 
            }
        }

        public IActionResult OnPostAddToCart(int id)
        {
            var sanPham = _dbContext.SanPham.Find(id);

            var sessionCart = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);
            if (existingCartItem != null)
            {
                existingCartItem.SoLuong++;
            }
            else
            {
                var cartItem = new Cart
                {
                    ID = sanPham.ID,
                    Anh = sanPham.Anh,
                    Ten = sanPham.Ten,
                    SoLuong = 1,
                    Gia = sanPham.Gia
                };
                cart.Add(cartItem);
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            return RedirectToPage("/Index");
        }
    }
}
