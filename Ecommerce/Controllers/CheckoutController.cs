using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extension;
using Ecommerce.Helpper;
using Ecommerce.Models;
using Ecommerce.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly EcommerceContext  _context;
        public INotyfService _notyfService { get; }

        public CheckoutController(EcommerceContext context, INotyfService notyfService)
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

        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            CheckoutVM model = new CheckoutVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.CustomerName;
                model.Email = khachhang.CustomerEmail;
                model.Phone = khachhang.CustomerPhone;
                model.Address = khachhang.CustomerAddress;
            }
            ViewBag.Cart = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index()
        {
            //Lay ra gio hang de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            CheckoutVM model = new CheckoutVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                //model.FullName = khachhang.CustomerName;
                //model.Email = khachhang.CustomerEmail;
                //model.Phone = khachhang.CustomerPhone;
                //model.Address = khachhang.CustomerAddress;

                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //Khoi tao don hang
                    Order donhang = new Order();
                    donhang.CustomerId = model.CustomerId;
                    donhang.OrderAddress = model.Address;
                    donhang.OrderCreatedDate = DateTime.Now;
                    donhang.OrderStatus = "pending";//Don hang moi
                    donhang.OrderCompleteDate = DateTime.Now.AddDays(3);
                    donhang.PaymentStatus = "unpaid";
                    donhang.PaymentMethod = "";
                    donhang.TotalPrice = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    if (donhang.TotalPrice > 250000)
                    {
                        donhang.ShippingFee = 0;
                        donhang.DiscountPrice = 20000;
                        donhang.GrandPrice = Convert.ToInt32(cart.Sum(x => x.TotalMoney)) + donhang.ShippingFee;
                    }
                    else
                    {
                        donhang.ShippingFee = 20000;
                        donhang.GrandPrice = Convert.ToInt32(cart.Sum(x => x.TotalMoney)) + donhang.ShippingFee;
                    }
                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang

                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.ProductName = item.product.ProductName;
                        orderDetail.ProductQuantity = item.amount;
                        orderDetail.ProductTotalPrice = donhang.TotalPrice;
                        orderDetail.ProductPrice = item.product.ProductDiscountPrice;
                        _context.Add(orderDetail);
                        _context.SaveChanges();
                    }
                    
                    //Clear gio hang
                    HttpContext.Session.Remove("Cart");
                    //Xuat thong bao
                    _notyfService.Success("Đặt hàng thành công!");
                    //Cap nhat thong tin khach hang
                    return RedirectToAction("Success");
                }
            }
            catch
            {
                ViewBag.Cart = cart;
                return View(model);
            }
            ViewBag.Cart = cart;
            return View(model);
        }

        [Route("order-success.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("AccountId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = "/order-success.html" });
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                var donhang = _context.Orders
                    .Where(x => x.CustomerId == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.OrderCreatedDate)
                    .FirstOrDefault();
                CheckoutSuccessVM successVM = new CheckoutSuccessVM();
                successVM.OrderID = donhang.OrderId;
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }
    }
}
