using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Models;
using Ecommerce.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class OrderViewController : Controller
    {
        private readonly EcommerceContext _context;
        public OrderViewController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("AccountId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Account");
                }
                var khachhang = _context.Customers
                    .AsNoTracking()
                    .SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                if (khachhang == null)
                {
                    return NotFound();
                }

                var donhang = await _context.Orders
                    .FirstOrDefaultAsync(m => m.OrderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);

                if (donhang == null)
                {
                    return NotFound();
                }

                var chitietdonhang = _context.OrderDetails
                    .Include(x => x.Product)
                    .AsNoTracking()
                    .Where(x => x.OrderId == id)
                    .OrderBy(x => x.OrderDetailId)
                    .ToList();
                OrderView donHang = new OrderView();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;

                return PartialView("Details", donHang);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
