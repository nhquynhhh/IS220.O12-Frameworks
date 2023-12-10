using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Microsoft.Extensions.Hosting;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBlogController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notyfService { get; }
        private readonly EcommerceContext _context;

        public AdminBlogController(IWebHostEnvironment webHostEnvironment, INotyfService notyfService, EcommerceContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
            _context = context;
        }

        // GET: Admin/AdminBlog
        public async Task<IActionResult> Index()
        {
              return _context.Blogs != null ? 
                          View(await _context.Blogs.ToListAsync()) :
                          Problem("Entity set 'EcommerceContext.Blogs'  is null.");
        }

        // GET: Admin/AdminBlog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/AdminBlog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminBlog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("BlogId,BlogTitle,BlogImage,BlogSummary,BlogContent,BlogCreatedDate,BlogModifiedDate,BlogAuthor")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fileRealPath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Cập nhật tên tệp tin mới trong đối tượng Blog
                    string fileInfo = new FileInfo(fileRealPath).Name;
                    string filePath = "/uploads/" + fileInfo;
                    blog.BlogImage = filePath;
                }
                
                _context.Add(blog);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới bài đăng thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/AdminBlog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/AdminBlog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("BlogId,BlogTitle,BlogImage,BlogSummary,BlogContent,BlogCreatedDate,BlogModifiedDate,BlogAuthor")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var fileRealPath = Path.Combine(uploads, fileName);

                        using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // Cập nhật tên tệp tin mới trong đối tượng Blog
                        string fileInfo = new FileInfo(fileRealPath).Name;
                        string filePath = "/uploads/" + fileInfo;
                        blog.BlogImage = filePath;
                    }
                    
                    _context.Update(blog);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/AdminBlog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/AdminBlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'EcommerceContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.Blogs?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}
