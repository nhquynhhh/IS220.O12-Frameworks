using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly EcommerceContext _context;
        public SearchController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.Products.AsNoTracking()
                                  .Include(a => a.ProductCategory)
                                  .Include(a => a.ProductSubCategory)
                                  .Include(a => a.ProductBrand)
                                  .OrderBy(x => x.ProductId)
                                  .ToList();
            }
            if (keyword != null)
            {
                keyword = keyword.ToLower();
                ls = _context.Products.AsNoTracking()
                                  .Include(a => a.ProductCategory)
                                  .Include(a => a.ProductSubCategory)
                                  .Include(a => a.ProductBrand)
                                  .Where(x => x.ProductName.ToLower().Contains(keyword))
                                  .OrderBy(x => x.ProductId)
                                  .Take(10)
                                  .ToList();
            }
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }

        [HttpPost]
        public IActionResult FindCustomer(string keyword)
        {
            List<Customer> ls = new List<Customer>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.Customers
                        .AsNoTracking()
                        .Include(c => c.Account)
                        .Include(c => c.Orders)
                        .OrderBy(x => x.CustomerId)
                        .ToList();
            }

            if(keyword != null)
            {
                keyword = keyword.ToLower();
                ls = _context.Customers
                                    .AsNoTracking()
                                    .Include(c => c.Account)
                                    .Include(c => c.Orders)
                                    .Where(x => x.CustomerName.ToLower().Contains(keyword))
                                    .OrderBy(x => x.CustomerId)
                                    .Take(10)
                                    .ToList();

            }

            if (ls == null)
            {
                return PartialView("ListCustomerSearchPartial", null);
            }
            else
            {
                return PartialView("ListCustomerSearchPartial", ls);
            }
        }
    }
}
