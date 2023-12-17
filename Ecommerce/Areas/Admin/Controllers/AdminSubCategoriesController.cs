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
using Ecommerce.Helpper;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSubCategoriesController : Controller
    {
        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }
        public AdminSubCategoriesController(EcommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminSubCategories
        public IActionResult Index(int page = 1, int subCategoryID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<SubCategory> lsSubCat = new List<SubCategory>();

            lsSubCat = _context.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Products)
            .OrderBy(x => x.SubCategoryId).ToList();

            PagedList<SubCategory> models = new PagedList<SubCategory>(lsSubCat.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentSubCatID = subCategoryID;

            ViewBag.currentPage = pageNumber;

            var ecommerceContext = _context.SubCategories.Include(p => p.Products).Include(p => p.Category);
            return View(models);
        }

        // GET: Admin/AdminSubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: Admin/AdminSubCategories/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/AdminSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubCategoryId,SubCategoryName,SubCategoryDescription,ProductCount,CategoryId,SubCategoryCreatedDate,SubCategoryModifiedDate,IsActive,SubCategorySlug")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                subCategory.SubCategoryName = Utilities.ToTitleCase(subCategory.SubCategoryName);
                subCategory.ProductCount = 0;
                subCategory.SubCategoryCreatedDate = DateTime.Now;
                subCategory.SubCategoryModifiedDate = DateTime.Now;
                subCategory.IsActive = true;
                subCategory.SubCategorySlug = Utilities.SEOUrl(subCategory.SubCategoryName);

                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới danh mục thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: Admin/AdminSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: Admin/AdminSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubCategoryId,SubCategoryName,SubCategoryDescription,ProductCount,CategoryId,SubCategoryCreatedDate,SubCategoryModifiedDate,IsActive,SubCategorySlug")] SubCategory subCategory)
        {
            if (id != subCategory.SubCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subCategory.SubCategoryName = Utilities.ToTitleCase(subCategory.SubCategoryName);
                    subCategory.ProductCount = _context.Products.Count(p => p.ProductSubCategoryId == subCategory.SubCategoryId);
                    subCategory.SubCategoryModifiedDate = DateTime.Now;
                    subCategory.IsActive = true;
                    subCategory.SubCategorySlug = Utilities.SEOUrl(subCategory.SubCategoryName);

                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa danh mục thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.SubCategoryId))
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
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: Admin/AdminSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubCategories == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: Admin/AdminSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubCategories == null)
            {
                return Problem("Entity set 'EcommerceContext.SubCategories'  is null.");
            }
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
          return (_context.SubCategories?.Any(e => e.SubCategoryId == id)).GetValueOrDefault();
        }
    }
}
