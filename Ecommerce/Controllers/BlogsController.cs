using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core;

namespace Ecommerce.Controllers
{
    public class BlogsController : Controller
    {
        private readonly EcommerceContext _context;

        public BlogsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Blogs
        [Route("blogs.html", Name = ("Blog"))]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            var lsBlogs = _context.Blogs
            .AsNoTracking()
            .OrderBy(x => x.BlogCreatedDate);

            PagedList<Blog> models = new PagedList<Blog>(lsBlogs.AsQueryable(), pageNumber, pageSize);

            ViewBag.currentPage = pageNumber;

            return View(models);
        }

        // GET: Blogs/Details/5
        [Route("/Blog/{BlogSlug}-{id}.html", Name = "Blog Details")]
        public IActionResult Details(int id)
        {
            var blog = _context.Blogs
                .AsNoTracking()
                .SingleOrDefault(x => x.BlogId == id);

            if (blog == null)
            {
                return RedirectToAction("Index");
            }

            var lsBlogs = _context.Blogs
                .AsNoTracking()
                .Where(x => x.BlogId != id)
                .Take(3)
                .OrderBy(x => x.BlogCreatedDate).ToList();

            ViewBag.SuggestedBlog = lsBlogs;
            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        private bool BlogExists(int id)
        {
          return (_context.Blogs?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}
