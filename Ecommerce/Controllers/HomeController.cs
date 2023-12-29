using Ecommerce.Models;
using Ecommerce.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _context;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index()
        {
            HomeVM model = new HomeVM();

            var lsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .OrderByDescending(x => x.ProductCreatedDate)
                .ToList();

            var lsBlogs = _context.Blogs
                .AsNoTracking()
                .OrderByDescending(x => x.BlogCreatedDate)
                .Take(3)
                .ToList();

            ViewBag.SuggestedProducts = lsProducts;
            ViewBag.SuggestedBlogs = lsBlogs;

            return View(model);
        }

        [Route("contact.html", Name = "Hỗ trợ khách hàng")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("questions.html", Name = "Câu hỏi thường gặp")]
        public IActionResult Questions()
        {
            return View();
        }

        [Route("privacy.html", Name = "Chính sách bảo mật")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("terms-of-use.html", Name = "Điều khoản sử dụng")]
        public IActionResult TermsOfUse()
        {
            return View();
        }

        [Route("about-us.html", Name = "Giới thiệu")]
        public IActionResult About()
        {
            return View();
        }

        [Route("return-policy.html", Name = "Chính sách đổi trả")]
        public IActionResult Return()
        {
            return View();
        }

        [Route("delivery-policy.html", Name = "Chính sách vận chuyển")]
        public IActionResult Delivery()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}