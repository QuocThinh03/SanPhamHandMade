using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuocThinh.Models;

namespace QuocThinh.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
        }

        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("UserID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TaiKhoan == Input.TaiKhoan && u.MatKhau == Input.MatKhau);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return Page();
            }

            HttpContext.Session.SetInt32("UserID", (int)user.ID);

            if (user.Quyen == 1)
            {
                return RedirectToPage("/Main");
            }
            else if (user.Quyen == 2)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
