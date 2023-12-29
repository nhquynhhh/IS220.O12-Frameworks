using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extension;
using Ecommerce.Models;
using Ecommerce.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }

        public CartController(EcommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public List<CartItem> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if (cart == default(List<CartItem>))
                {
                    cart = new List<CartItem>();
                }
                return cart;
            }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> cart = Cart;

            try
            {
                //Them san pham vao gio hang
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null) // da co => cap nhat so luong
                {
                    item.amount = item.amount + amount.Value;
                    //luu lai session
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                }
                else
                {
                    Product sanpham = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = sanpham
                    };
                    cart.Add(item);//Them vao gio
                }

                //Luu lai Session
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                _notyfService.Success("Thêm sản phẩm thành công!");
                return Json(new { success = true });
            }
            catch
            {
                _notyfService.Error("Thêm sản phẩm không thành công!");
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && amount.HasValue) // da co -> cap nhat so luong
                    {
                        item.amount = amount.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> cart = Cart;
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    cart.Remove(item);
                }
                //luu lai session
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                _notyfService.Success("Xóa sản phẩm thành công!");
                return Json(new { success = true });
            }
            catch
            {
                _notyfService.Error("Xóa sản phẩm không thành công!");
                return Json(new { success = false });
            }
        }


        [Route("cart.html", Name = "Giỏ hàng")]
        public IActionResult Index()
        {
            return View(Cart);
        }
    }
}
