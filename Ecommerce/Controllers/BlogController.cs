using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
