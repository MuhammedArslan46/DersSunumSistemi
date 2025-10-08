using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using DersSunumSistemi.Services;
using DersSunumSistemi.Models;
using DersSunumSistemi.Data;
using Microsoft.EntityFrameworkCore;

namespace DersSunumSistemi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string? returnUrl = null)
        {
            var user = await _authService.AuthenticateAsync(userName, password);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName),
                new Claim("Role", user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Rol bazlı yönlendirme
            if (user.Role == UserRole.Admin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.Role == UserRole.Instructor)
            {
                return RedirectToAction("Dashboard", "Instructor");
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Departments = await _context.Departments
                .Include(d => d.Faculty)
                .ThenInclude(f => f!.Institution)
                .OrderBy(d => d.Faculty!.Institution.Name)
                .ThenBy(d => d.Faculty!.Name)
                .ThenBy(d => d.Name)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string email, string password, string confirmPassword, string fullName, int departmentId)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor!";
                ViewBag.Departments = await _context.Departments
                    .Include(d => d.Faculty)
                    .ThenInclude(f => f!.Institution)
                    .ToListAsync();
                return View();
            }

            var user = await _authService.RegisterAsync(userName, email, password, fullName, UserRole.Student, departmentId);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya email zaten kullanımda!";
                ViewBag.Departments = await _context.Departments
                    .Include(d => d.Faculty)
                    .ThenInclude(f => f!.Institution)
                    .ToListAsync();
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
