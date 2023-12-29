using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceContext _context;
        public ProductController(EcommerceContext context)
        {
            _context = context;
        }

        [Route("products.html", Name = ("Sản phẩm"))]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 20;
                var lsProducts = _context.Products
                    .AsNoTracking()
                    .OrderBy(x => x.ProductCreatedDate);
                var lsCats = _context.Categories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.CategoryName)
                    .ToList();
                var lsBrands = _context.Brands
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.BrandName)
                    .ToList();
                ViewBag.Category = lsCats;
                ViewBag.Brand = lsBrands;

                PagedList<Product> models = new PagedList<Product>(lsProducts, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [Route("Category/{CategorySlug}", Name = ("Danh sách danh mục cha"))]
        public IActionResult ListCat(string CategorySlug, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var cat = _context.Categories
                    .AsNoTracking()
                    .SingleOrDefault(x => x.CategorySlug == CategorySlug);

                if (cat == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.ProductCategoryId == cat.CategoryId)
                    .OrderByDescending(x => x.ProductCreatedDate);
                var lsCats = _context.Categories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.CategoryName)
                    .ToList();
                var lsSubCats = _context.SubCategories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.SubCategoryName)
                    .ToList();
                var lsBrands = _context.Brands
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.BrandName)
                    .ToList();
                ViewBag.Category = lsCats;
                ViewBag.SubCategory = lsSubCats;
                ViewBag.Brand = lsBrands;

                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                 ViewBag.CurrentPage = page;
                 ViewBag.CurrentCat = cat;
                    return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("SubCategory/{SubCategorySlug}", Name = ("Danh sách danh mục con"))]
        public IActionResult ListSubCat(string SubCategorySlug, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var subCat = _context.SubCategories
                    .AsNoTracking()
                    .SingleOrDefault(x => x.SubCategorySlug == SubCategorySlug);

                if (subCat == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.ProductSubCategoryId == subCat.SubCategoryId)
                    .OrderByDescending(x => x.ProductCreatedDate);
                var lsCats = _context.Categories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.CategoryName)
                    .ToList();
                var lsSubCats = _context.SubCategories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.SubCategoryName)
                    .ToList();
                var lsBrands = _context.Brands
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.BrandName)
                    .ToList();
                ViewBag.Category = lsCats;
                ViewBag.SubCategory = lsSubCats;
                ViewBag.Brand = lsBrands;

                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentSubCat = subCat;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Brand/{BrandName}", Name = ("Danh sách thương hiệu"))]
        public IActionResult ListBrands(string BrandName, int page = 1)
        {
            try
            {
                var pageSize = 20;
                var brand = _context.Brands
                    .AsNoTracking()
                    .SingleOrDefault(x => x.BrandName == BrandName);

                if (brand == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.ProductBrandId == brand.BrandId)
                    .OrderByDescending(x => x.ProductCreatedDate);
                var lsCats = _context.Categories
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.CategoryName)
                    .ToList();
                var lsBrands = _context.Brands
                    .AsNoTracking()
                    .Take(6)
                    .OrderBy(x => x.BrandName)
                    .ToList();
                ViewBag.Category = lsCats;
                ViewBag.Brand = lsBrands;

                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentBrand = brand;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Product}/{ProductSlug}-{id}.html", Name = ("ProductDetails"))]
        public IActionResult Detail(int id)
        {
            try
            {
                var product = _context.Products
                    .Include(x => x.ProductCategory)
                    .Include(x => x.ProductSubCategory)
                    .Include(x => x.ProductBrand)
                    .FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsProduct = _context.Products
                    .AsNoTracking()
                    .Where(x => x.ProductCategoryId == product.ProductCategoryId && x.ProductId != id && x.IsActive == true)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(4)
                    .OrderBy(x => x.ProductId)
                    .ToList();

                ViewBag.SanPham = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
