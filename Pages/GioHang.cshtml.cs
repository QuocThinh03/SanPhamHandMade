using QuocThinh.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace QuocThinh.Pages
{
    public class GioHangModel : PageModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoLuong { get; set; }
        public string NavCssClass { get; set; }
        public void OnGet()
        {
            NavCssClass = "category-style-2";
            LoadCart();
        }
        private void LoadCart()
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
        }
        public IActionResult OnPost(string action, int productId, int newQuantity)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            if (sessionCart != null)
            {
                var cartItems = JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

                var cartItem = cartItems.FirstOrDefault(item => item.ID == productId);
                if (cartItem != null)
                {
                    if (action == "plus")
                    {
                        cartItem.SoLuong = newQuantity;
                    }
                    else if (action == "minus")
                    {
                        if (newQuantity == 0)
                        {
                            cartItems.Remove(cartItem);
                        }
                        else
                        {
                            cartItem.SoLuong = newQuantity;
                        }
                    }

                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
                }
            }

            return RedirectToPage("/GioHang");
        }
        public IActionResult OnPostPlusCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            existingCartItem.SoLuong++;

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            return RedirectToPage("/GioHang");
        }

        public IActionResult OnPostMinusCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            if (existingCartItem != null)
            {
                existingCartItem.SoLuong--;

                if (existingCartItem.SoLuong <= 0)
                {
                    cart.Remove(existingCartItem);
                }

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToPage("/GioHang");
        }

        public IActionResult OnPostDeleteCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            var cart = string.IsNullOrEmpty(sessionCart) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

            var existingCartItem = cart.FirstOrDefault(item => item.ID == id);

            if (existingCartItem != null)
            {
                cart.Remove(existingCartItem);

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToPage("/GioHang");
        }
    }
}
