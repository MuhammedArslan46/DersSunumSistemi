using Microsoft.AspNetCore.Mvc;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace DersSunumSistemi.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Admin Ana Sayfa
        public async Task<IActionResult> Index()
        {
            // Session kontrolü - Admin girişi yaptı mı?
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewBag.CategoryCount = await _context.Categories.CountAsync();
            ViewBag.CourseCount = await _context.Courses.CountAsync();
            ViewBag.PresentationCount = await _context.Presentations.CountAsync();

            return View();
        }

        // Login Sayfası (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login İşlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password && u.IsAdmin == true);

            if (user != null)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("UserName", user.FullName);
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Çıkış
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}