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
    public class AdminCustomersController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notyfService { get; }

        public AdminCustomersController(IWebHostEnvironment webHostEnvironment, EcommerceContext context, INotyfService notyfService)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCustomers
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsCustomers = _context.Customers
                .AsNoTracking()
                .Include(c => c.Account)
                .Include(c => c.Orders)
                .OrderBy(x => x.CustomerId);

            PagedList<Customer> models = new PagedList<Customer>(lsCustomers, pageNumber, pageSize);

            ViewBag.currentPage = pageNumber;

            var ecommerceContext = _context.Customers
                .Include(c => c.Account)
                .Include(c => c.Orders);
            return View(models);
        }

        // GET: Admin/AdminCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,AccountId,CustomerName,CustomerEmail,CustomerPhone,CustomerAddress,CustomerAvatar,CustomerJoinDate,CustomerOrderQuantity,CustomerBankAccount,CustomerBank,IsActive")] Customer customer, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                customer.CustomerName = Utilities.ToTitleCase(customer.CustomerName);
                if (file != null && file.Length > 0)
                {
                    // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "customers");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fileRealPath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Cập nhật tên tệp tin mới trong đối tượng Blog
                    string fileInfo = new FileInfo(fileRealPath).Name;
                    string filePath = "/uploads/customers/" + fileInfo;
                    customer.CustomerAvatar = filePath;
                }
                customer.CustomerAddress = Utilities.ToTitleCase(customer.CustomerAddress);
                customer.CustomerJoinDate = DateTime.Now;
                customer.CustomerOrderQuantity = 0;
                customer.IsActive = true;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm khách hàng thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // POST: Admin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,AccountId,CustomerName,CustomerEmail,CustomerPhone,CustomerAddress,CustomerAvatar,CustomerJoinDate,CustomerOrderQuantity,CustomerBankAccount,CustomerBank,IsActive")] Customer customer, IFormFile file)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.CustomerName = Utilities.ToTitleCase(customer.CustomerName);
                    if (file != null && file.Length > 0)
                    {
                        // Lưu tệp tin vào thư mục hoặc lưu trữ bạn mong muốn
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "customers");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var fileRealPath = Path.Combine(uploads, fileName);

                        using (var fileStream = new FileStream(fileRealPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        // Cập nhật tên tệp tin mới trong đối tượng Blog
                        string fileInfo = new FileInfo(fileRealPath).Name;
                        string filePath = "/uploads/customers/" + fileInfo;
                        customer.CustomerAvatar = filePath;
                    }
                    customer.CustomerAddress = Utilities.ToTitleCase(customer.CustomerAddress);
                    customer.IsActive = true;

                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thông tin khách hàng thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", customer.AccountId);
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'EcommerceContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa khách hàng thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
