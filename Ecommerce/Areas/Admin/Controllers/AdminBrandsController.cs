using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;
using System.Reflection.Metadata;
using Ecommerce.Helpper;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBrandsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }

        public AdminBrandsController(IWebHostEnvironment webHostEnvironment, INotyfService notyfService, EcommerceContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminBrands
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsBrands = _context.Brands
                .AsNoTracking()
                .OrderBy(x => x.BrandId);

            PagedList<Brand> models = new PagedList<Brand>(lsBrands, pageNumber, pageSize);

            ViewBag.currentPage = pageNumber;

            var ecommerceContext = _context.Brands;
            return View(models);
        }

        // GET: Admin/AdminBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/AdminBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,BrandName,BrandOrigin,BrandImage")] Brand brand, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "brands");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fileRealPath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Cập nhật tên tệp tin mới trong đối tượng Blog
                    string fileInfo = new FileInfo(fileRealPath).Name;
                    string filePath = "/uploads/brands/" + fileInfo;
                    brand.BrandImage = filePath;
                }

                brand.BrandName = brand.BrandName.ToUpper();
                brand.BrandOrigin = Utilities.ToTitleCase(brand.BrandOrigin);

                _context.Add(brand);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm thương hiệu thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/AdminBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/AdminBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName,BrandOrigin,BrandImage")] Brand brand, IFormFile file)
        {
            if (id != brand.BrandId)
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
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "brands");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var fileRealPath = Path.Combine(uploads, fileName);

                        using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // Cập nhật tên tệp tin mới trong đối tượng Blog
                        string fileInfo = new FileInfo(fileRealPath).Name;
                        string filePath = "/uploads/brands/" + fileInfo;
                        brand.BrandImage = filePath;
                    }

                    brand.BrandName = brand.BrandName.ToUpper();
                    brand.BrandOrigin = Utilities.ToTitleCase(brand.BrandOrigin);

                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thương hiệu thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BrandId))
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
            return View(brand);
        }

        // GET: Admin/AdminBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Admin/AdminBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'EcommerceContext.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thương hiệu thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
          return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        }
    }
}
