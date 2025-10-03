using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using DersSunumSistemi.Services;
using DersSunumSistemi.Models;

namespace DersSunumSistemi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string email, string password, string confirmPassword, string fullName)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor!";
                return View();
            }

            var user = await _authService.RegisterAsync(userName, email, password, fullName, UserRole.Student);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya email zaten kullanımda!";
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
