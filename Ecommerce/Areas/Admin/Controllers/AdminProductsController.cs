using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly EcommerceContext _context;

        public INotyfService _notyfService { get; }

        public AdminProductsController(EcommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.ProductSubCategory)
                .Include(x => x.ProductCategory)
                .OrderByDescending(x => x.ProductId);
            PagedList<Product> models = new PagedList<Product>(lsProducts, pageNumber, pageSize);

            ViewBag.currentPage = pageNumber;

            var ecommerceContext = _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductSubCategory);
            return View(models);
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductSubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductBrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId");
            ViewData["ProductSubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryId");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductBrandId,ProductCategoryId,ProductSubCategoryId,ProductOriginalPrice,ProductDiscountPrice,ProductInfo,ProductBarcode,ProductInStock,ProductSoldQuantity,ProductImage,ProductSideImage1,ProductSideImage2,ProductSideImage3,ProductCreatedDate,ProductModifiedDate,IsFlashSale,IsActive")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductBrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.ProductBrandId);
            ViewData["ProductSubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryId", product.ProductSubCategoryId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductBrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.ProductBrandId);
            ViewData["ProductSubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryId", product.ProductSubCategoryId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductBrandId,ProductCategoryId,ProductSubCategoryId,ProductOriginalPrice,ProductDiscountPrice,ProductInfo,ProductBarcode,ProductInStock,ProductSoldQuantity,ProductImage,ProductSideImage1,ProductSideImage2,ProductSideImage3,ProductCreatedDate,ProductModifiedDate,IsFlashSale,IsActive")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductBrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.ProductBrandId);
            ViewData["ProductSubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryId", product.ProductSubCategoryId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductSubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'EcommerceContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
