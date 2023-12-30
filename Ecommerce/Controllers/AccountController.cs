using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extension;
using Ecommerce.Helpper;
using Ecommerce.Models;
using Ecommerce.ModelViews;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly EcommerceContext _context;
        public INotyfService _notyfService { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(IWebHostEnvironment webHostEnvironment, EcommerceContext context, INotyfService notyfService)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _notyfService = notyfService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers
                    .AsNoTracking()
                    .SingleOrDefault(x => x.CustomerPhone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + " đã được sử dụng.");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers
                    .AsNoTracking()
                    .SingleOrDefault(x => x.CustomerEmail.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng.");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register.html", Name = "Đăng ký tài khoản")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register.html", Name = "Đăng ký tài khoản")]
        public async Task<IActionResult> Register(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();

                    Customer khachhang = new Customer
                    {
                        CustomerPhone = taikhoan.Phone.Trim().ToLower(),
                        CustomerEmail = taikhoan.AccountEmail.Trim(),
                        CustomerName = taikhoan.AccountName.Titleize(),
                        IsActive = true,
                        CustomerJoinDate = DateTime.Now,
                        CustomerPassword = (taikhoan.AccountPassword + salt.Trim()).ToMD5(),
                        Salt = salt,
                        CustomerAvatar = "/uploads/customers/default.jpg",
                        CustomerAddress = "Việt Nam"
                    };
                    Account account = new Account
                    {
                        AccountEmail = taikhoan.AccountEmail.Trim(),
                        AccountName = taikhoan.AccountName.Titleize(),
                        IsActive = true,
                        AccountRegisterDate = DateTime.Now,
                        AccountPassword = (taikhoan.AccountPassword + salt.Trim()).ToMD5(),
                        Salt = salt,
                        AccountRoleId = 2
                    };

                    try
                    {
                        _context.Add(khachhang);
                        _context.Add(account);
                        await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("AccountId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.AccountName),
                            new Claim("AccountId", account.AccountId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Detail", "Account");
                    }
                    catch
                    {
                        return RedirectToAction("Register", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [AllowAnonymous]
        [Route("login.html", Name = "Đăng nhập")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "Đăng nhập")]
        public async Task<IActionResult> Login(LoginVM taikhoan, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(taikhoan.Email);
                    if (!isEmail) return View(taikhoan);

                    var account = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountEmail.Trim() == taikhoan.Email);

                    if (account == null) return RedirectToAction("Register");
                    string pass = (taikhoan.Password + account.Salt.Trim()).ToMD5();
                    if (account.AccountPassword != pass)
                    {
                        _notyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(taikhoan);
                    }
                    //kiem tra xem account co bi disable hay khong

                    if (account.IsActive == false)
                    {
                        _notyfService.Error("Tài khoản hiện không hoạt động");
                        return RedirectToAction("Login", "Account");
                    }

                    //Luu Session MaKh
                    HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("AccountId");

                    //Identity
                    
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.AccountName),
                            new Claim("AccountId", account.AccountId.ToString())
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        if(account.AccountRoleId == 2)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if(account.AccountRoleId == 1)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch
            {
                return RedirectToAction("Register", "Account");
            }
            return View(taikhoan);
        }

        [HttpGet]
        [Route("log-out.html", Name = "Đăng xuất")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("AccountId");
            _notyfService.Success("Đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }

        [Route("my-account.html", Name = "Tài khoản của tôi")]
        public IActionResult Detail()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.Orders
                        .Include(x => x.OrderDetails)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == khachhang.CustomerId)
                        .OrderByDescending(x => x.OrderCreatedDate)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }

            }
            return RedirectToAction("Login");
        }
    }
}
