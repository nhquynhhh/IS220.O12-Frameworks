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
    public class AdminDiscountsController : Controller
    {
        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }

        public AdminDiscountsController(EcommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDiscounts
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsDiscounts = _context.Discounts
                .AsNoTracking()
                .OrderBy(x => x.DiscountId);

            PagedList<Discount> models = new PagedList<Discount>(lsDiscounts, pageNumber, pageSize);

            ViewBag.currentPage = pageNumber;

            var ecommerceContext = _context.Discounts;
            return View(models);
        }

        // GET: Admin/AdminDiscounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Admin/AdminDiscounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminDiscounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountId,DiscountCode,DiscountName,DiscountDescription,DiscountQuantity,DiscountUsed,DiscountType,DiscountValue,IsActive,DiscountStartDate,DiscountEndDate,DiscountCreatedDate,DiscountModifiedDate")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                discount.DiscountCode = discount.DiscountCode.ToUpper();
                discount.DiscountName = Utilities.ToTitleCase(discount.DiscountName);
                discount.DiscountType = "fixed";
                discount.DiscountUsed = 0;
                discount.DiscountCreatedDate = DateTime.Now;
                discount.DiscountModifiedDate = DateTime.Now;

                _context.Add(discount);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm khuyến mãi thành công!");
                return RedirectToAction(nameof(Index));
            }

            ViewData["DiscountType"] = new SelectList(_context.Discounts);
            return View(discount);
        }

        // GET: Admin/AdminDiscounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Admin/AdminDiscounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountId,DiscountCode,DiscountName,DiscountDescription,DiscountQuantity,DiscountUsed,DiscountType,DiscountValue,IsActive,DiscountStartDate,DiscountEndDate,DiscountCreatedDate,DiscountModifiedDate")] Discount discount)
        {
            if (id != discount.DiscountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    discount.DiscountCode = discount.DiscountCode.ToUpper();
                    discount.DiscountName = Utilities.ToTitleCase(discount.DiscountName);
                    discount.DiscountModifiedDate = DateTime.Now;

                    _context.Update(discount);
                    _notyfService.Success("Chỉnh sửa khuyến mãi thành công!");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.DiscountId))
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
            return View(discount);
        }

        // GET: Admin/AdminDiscounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Admin/AdminDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Discounts == null)
            {
                return Problem("Entity set 'EcommerceContext.Discounts'  is null.");
            }
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa khuyến mãi thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
          return (_context.Discounts?.Any(e => e.DiscountId == id)).GetValueOrDefault();
        }
    }
}
