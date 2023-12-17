using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core;
using Ecommerce.Helpper;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }

        public AdminCategoryController(EcommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCategory
        public IActionResult Index(int page = 1, int categoryID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<Category> lsCategories = new List<Category>();

                lsCategories = _context.Categories
                .AsNoTracking()
                .Include(x => x.SubCategories)
                .Include(x => x.Products)
                .OrderBy(x => x.CategoryId).ToList();

            PagedList<Category> models = new PagedList<Category>(lsCategories.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCatID = categoryID;

            ViewBag.currentPage = pageNumber;

            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            var ecommerceContext = _context.Categories.Include(p => p.Products).Include(p => p.SubCategories);
            return View(models);
        }


        // GET: Admin/AdminCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,SubCategoryCount,ProductCount,CategoryCreatedDate,CategoryModifiedDate,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryName = Utilities.ToTitleCase(category.CategoryName);
                category.SubCategoryCount = 0;
                category.ProductCount = 0;
                category.CategoryCreatedDate = DateTime.Now;
                category.CategoryModifiedDate = DateTime.Now;
                category.IsActive = true;
                category.CategorySlug = Utilities.SEOUrl(category.CategoryName);

                _context.Add(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới danh mục thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,SubCategoryCount,ProductCount,CategoryCreatedDate,CategoryModifiedDate,IsActive")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.CategoryName = Utilities.ToTitleCase(category.CategoryName);
                    category.SubCategoryCount = _context.SubCategories.Count(s => s.CategoryId == category.CategoryId);
                    category.ProductCount = _context.Products.Count(p => p.ProductCategoryId == category.CategoryId);
                    category.CategoryModifiedDate = DateTime.Now;
                    category.IsActive = true;
                    category.CategorySlug = Utilities.SEOUrl(category.CategoryName);

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa danh mục thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Admin/AdminCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'EcommerceContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
