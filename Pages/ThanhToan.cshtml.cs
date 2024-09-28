using QuocThinh.Pages;
using QuocThinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;

namespace QuocThinh.Pages
{
    public class ThanhToanModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private IConfiguration _configuration;
        public ThanhToanModel(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public List<Cart> CartItems { get; set; }
        public decimal TongTien { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public decimal SoLuong { get; set; }
        public string NavCssClass { get; set; }
        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            public string Ten { get; set; }
            public string DiaChi { get; set; }
        }
        public void OnGet()
        {
            NavCssClass = "category-style-2";
            LoadCheckOut();
        }

        private void LoadCheckOut()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            if (sessionCart != null)
            {
                CartItems = JsonConvert.DeserializeObject<List<Cart>>(sessionCart);
                TongTien = CartItems.Sum(item => item.Gia * item.SoLuong);
                SoLuong = CartItems.Sum(item => item.SoLuong);
            }
            else
            {
                CartItems = new List<Cart>();
                TongTien = 0;
            }

            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId != null)
            {
                NguoiDung = _dbContext.NguoiDung.FirstOrDefault(u => u.ID == userId);
            }
            else
            {
                NguoiDung = new NguoiDung { Ten = "", DiaChi = "" };
            }
        }

        public IActionResult OnPost()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            CartItems = JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            DonHang donHang = new DonHang
            {
                Ten = Input.Ten,
                NgayDat = DateTime.Now,
                DiaChi = Input.DiaChi
            };

            _dbContext.DonHang.Add(donHang);
            _dbContext.SaveChanges();

            long totalAmount = 0;

            foreach (var chiTiet in CartItems)
            {
                ChiTietDonHang ct = new ChiTietDonHang
                {
                    IDSanPham = chiTiet.ID,
                    SoLuong = chiTiet.SoLuong,
                    Gia = chiTiet.Gia,
                    IDDonHang = donHang.ID,
                };
                _dbContext.ChiTietDonHang.Add(ct);

                totalAmount += chiTiet.Gia * chiTiet.SoLuong;
            }

            _dbContext.SaveChanges();

            HttpContext.Session.Remove("Cart");

            var model = new PaymentInformationModel
            {
                Amount = totalAmount,
                OrderId = donHang.ID.ToString(),
                OrderDescription = "Hàng dễ vỡ",
                OrderType = "other",
                Url = "/CallBack",
            };

            var paymentUrl = CreatePaymentUrl(model, HttpContext);

            return Redirect(paymentUrl);

        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan cho don hang: " + model.OrderId);
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", "https://localhost:5265/callback");
            pay.AddRequestData("vnp_TxnRef", model.OrderId.ToString());
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
    }
}
