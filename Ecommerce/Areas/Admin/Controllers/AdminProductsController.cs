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
using Ecommerce.Helpper;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Reflection.Metadata;

#nullable disable

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public INotyfService _notyfService { get; }

        public AdminProductsController(IWebHostEnvironment webHostEnvironment, EcommerceContext context, INotyfService notyfService)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int productCategoryID = 0, int productSubCategoryID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<Product> lsProducts = new List<Product>();

            if (productCategoryID != 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.ProductCategoryId == productCategoryID)
                .Include(x => x.ProductSubCategory)
                .Include(x => x.ProductCategory)
                .OrderBy(x => x.ProductId).ToList();
            }
            else if (productSubCategoryID != 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.ProductSubCategoryId == productSubCategoryID)
                .Include(x => x.ProductSubCategory)
                .Include(x => x.ProductCategory)
                .OrderBy(x => x.ProductId).ToList();
            }
            else if (productSubCategoryID == 0 || productCategoryID == 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.ProductSubCategory)
                .Include(x => x.ProductCategory)
                .OrderBy(x => x.ProductId).ToList();
            }

            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCatID = productCategoryID;
            ViewBag.CurrentSubCatID = productSubCategoryID;

            ViewBag.currentPage = pageNumber;

            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["SubCategory"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");

            var ecommerceContext = _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductCategory);
            return View(models);
        }

        public IActionResult FilterCategory(int productCategoryID = 0)
        {
            var url = $"/Admin/AdminProducts?productCategoryID={productCategoryID}";
            if (productCategoryID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url});
        }

        public IActionResult FilterSubCategory(int productSubCategoryID = 0)
        {
            var url = $"/Admin/AdminProducts?productSubCategoryID={productSubCategoryID}";
            if (productSubCategoryID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url});
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
                .Include(p => p.ProductCategory)
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
            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["SubCategory"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile fImg, [Bind("ProductId,ProductName,ProductBrandId,ProductCategoryId,ProductSubCategoryId,ProductOriginalPrice,ProductDiscountPrice,ProductInfo,ProductBarcode,ProductInStock,ProductSoldQuantity,ProductImage,ProductCreatedDate,ProductModifiedDate,IsFlashSale,IsActive,ProductSlug")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fImg != null && fImg.Length > 0)
                {
                    // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fImg.FileName);
                    var fileRealPath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                    {
                        await fImg.CopyToAsync(fileStream);
                    }
                    // Cập nhật tên tệp tin mới trong đối tượng Blog
                    string fileInfo = new FileInfo(fileRealPath).Name;
                    string filePath = "/uploads/products/" + fileInfo;
                    product.ProductImage = filePath;
                }

                product.ProductSlug = Utilities.SEOUrl(product.ProductName);
                product.IsFlashSale = false;
                product.ProductCreatedDate = DateTime.Now;
                product.ProductModifiedDate = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.ProductBrandId);
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.ProductCategoryId);
            ViewData["SubCategory"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", product.ProductSubCategoryId);
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

            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.ProductBrandId);
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.ProductCategoryId);
            ViewData["SubCategory"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", product.ProductSubCategoryId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductBrandId,ProductCategoryId,ProductSubCategoryId,ProductOriginalPrice,ProductDiscountPrice,ProductInfo,ProductBarcode,ProductInStock,ProductSoldQuantity,ProductImage,ProductCreatedDate,ProductModifiedDate,IsFlashSale,IsActive,ProductSlug")] Product product, IFormFile fImg)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fImg != null && fImg.Length > 0)
                    {
                        // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fImg.FileName);
                        var fileRealPath = Path.Combine(uploads, fileName);

                        using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                        {
                            await fImg.CopyToAsync(fileStream);
                        }
                        // Cập nhật tên tệp tin mới trong đối tượng Blog
                        string fileInfo = new FileInfo(fileRealPath).Name;
                        string filePath = "/uploads/products/" + fileInfo;
                        product.ProductImage = filePath;
                    }

                    product.ProductSlug = Utilities.SEOUrl(product.ProductName);
                    product.ProductModifiedDate = DateTime.Now;

                    _context.Update(product);
                    _notyfService.Success("Chỉnh sửa sản phẩm thành công");
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
            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.ProductBrandId);
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.ProductCategoryId);
            ViewData["SubCategory"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", product.ProductSubCategoryId);
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
                .Include(p => p.ProductCategory)
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
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
