using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;
using DersSunumSistemi.Services;
using Microsoft.EntityFrameworkCore;

namespace DersSunumSistemi.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AdminController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // Admin Ana Sayfa
        public async Task<IActionResult> Index()
        {
            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.InstructorCount = await _context.Instructors.CountAsync();
            ViewBag.DepartmentCount = await _context.Departments.CountAsync();
            ViewBag.CategoryCount = await _context.Categories.CountAsync();
            ViewBag.CourseCount = await _context.Courses.CountAsync();
            ViewBag.PresentationCount = await _context.Presentations.CountAsync();
            ViewBag.TotalViews = await _context.Presentations.SumAsync(p => p.ViewCount);

            return View();
        }

        // USERS MANAGEMENT
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Include(u => u.Instructor)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userName, string email, string password, string fullName, UserRole role, int? departmentId, string? title)
        {
            var user = await _authService.RegisterAsync(userName, email, password, fullName, role);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya email zaten kullanımda!";
                ViewBag.Departments = await _context.Departments.ToListAsync();
                return View();
            }

            // Eğer Instructor ise, Instructor kaydı oluştur
            if (role == UserRole.Instructor && departmentId.HasValue)
            {
                var instructor = new Instructor
                {
                    UserId = user.Id,
                    FullName = fullName,
                    Email = email,
                    Title = title,
                    DepartmentId = departmentId.Value,
                    CreatedDate = DateTime.Now
                };
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Users));
        }

        // DEPARTMENTS
        public async Task<IActionResult> Departments()
        {
            var departments = await _context.Departments
                .Include(d => d.Instructors)
                .ToListAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(Department model)
        {
            _context.Departments.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Departments));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Departments));
        }

        // STATISTICS
        public async Task<IActionResult> Statistics()
        {
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalInstructors = await _context.Instructors.CountAsync(),
                TotalCourses = await _context.Courses.CountAsync(),
                TotalPresentations = await _context.Presentations.CountAsync(),
                TotalViews = await _context.Presentations.SumAsync(p => p.ViewCount),
                TotalDownloads = await _context.Presentations.SumAsync(p => p.DownloadCount),

                CoursesByCategory = await _context.Categories
                    .Include(c => c.Courses)
                    .Select(c => new { c.Name, Count = c.Courses.Count })
                    .ToListAsync(),

                TopInstructors = await _context.Instructors
                    .Include(i => i.Courses)
                    .ThenInclude(c => c.Presentations)
                    .OrderByDescending(i => i.Courses.SelectMany(c => c.Presentations).Sum(p => p.ViewCount))
                    .Take(10)
                    .Select(i => new { i.FullName, i.Title, CourseCount = i.Courses.Count })
                    .ToListAsync()
            };

            return View(stats);
        }
    }
}