using QuocThinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace QuocThinh.Pages
{
    public class ChiTietSanPhamModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public SanPham SanPham { get; set; }
        public string NavCssClass { get; set; }
        public ChiTietSanPhamModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            NavCssClass = "category-style-2";
            SanPham = await _dbContext.SanPham.FindAsync(id);

            if (SanPham == null)
            {
                return NotFound();
            }

            return Page();
        }

        public void OnPostAddToCart(int id, int quantity)
        {
            var sanPham = _dbContext.SanPham.Find(id);

            var sessionCart = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);
            if (existingCartItem != null)
            {
                existingCartItem.SoLuong += quantity;
            }
            else
            {
                var cartItem = new Cart
                {
                    ID = sanPham.ID,
                    Anh = sanPham.Anh,
                    Ten = sanPham.Ten,
                    SoLuong = quantity,
                    Gia = sanPham.Gia
                };
                cart.Add(cartItem);
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
    }
}
