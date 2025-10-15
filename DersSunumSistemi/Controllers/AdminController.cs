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
            ViewBag.Departments = await _context.Departments
                .Include(d => d.Faculty)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userName, string email, string password, string fullName, UserRole role, int? departmentId, int? studentDepartmentId, string? title, bool isActive = true, DateTime? startDate = null, DateTime? endDate = null)
        {
            // Öğrenci için bölüm ID'sini kullan
            int? finalDepartmentId = role == UserRole.Student ? studentDepartmentId : null;

            var user = await _authService.RegisterAsync(userName, email, password, fullName, role, finalDepartmentId);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya email zaten kullanımda!";
                ViewBag.Departments = await _context.Departments
                    .Include(d => d.Faculty)
                    .ToListAsync();
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
                    CreatedDate = DateTime.Now,
                    IsActive = isActive,
                    StartDate = startDate ?? DateTime.Now, // Eğer belirtilmemişse bugün
                    EndDate = endDate
                };
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = $"{fullName} başarıyla oluşturuldu!";
            return RedirectToAction(nameof(Users));
        }

        // INSTRUCTOR MANAGEMENT
        [HttpGet]
        public async Task<IActionResult> EditInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                TempData["Error"] = "Akademisyen bulunamadı!";
                return RedirectToAction(nameof(Users));
            }

            ViewBag.Departments = await _context.Departments
                .Include(d => d.Faculty)
                .ToListAsync();
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> EditInstructor(int id, string fullName, string email, string? phone, string? title, int departmentId, bool isActive, DateTime? startDate, DateTime? endDate)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                TempData["Error"] = "Akademisyen bulunamadı!";
                return RedirectToAction(nameof(Users));
            }

            instructor.FullName = fullName;
            instructor.Email = email;
            instructor.Phone = phone;
            instructor.Title = title;
            instructor.DepartmentId = departmentId;
            instructor.IsActive = isActive;
            instructor.StartDate = startDate;
            instructor.EndDate = endDate;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"{fullName} başarıyla güncellendi!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                TempData["Error"] = "Akademisyen bulunamadı!";
                return RedirectToAction(nameof(Users));
            }

            // Akademisyenin dersleri varsa uyarı
            if (instructor.Courses.Any())
            {
                TempData["Error"] = $"{instructor.FullName} adlı akademisyenin {instructor.Courses.Count} dersi var. Önce dersleri silmelisiniz!";
                return RedirectToAction(nameof(Users));
            }

            // Kullanıcıyı da sil
            if (instructor.User != null)
            {
                _context.Users.Remove(instructor.User);
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{instructor.FullName} başarıyla silindi!";
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

        // COURSE ASSIGNMENT TO INSTRUCTORS
        [HttpGet]
        public async Task<IActionResult> AssignCourses()
        {
            var instructors = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .Where(i => i.IsActive)
                .OrderBy(i => i.FullName)
                .ToListAsync();

            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Department)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Instructors = instructors;
            ViewBag.Courses = courses;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignCourse(int courseId, int instructorId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var instructor = await _context.Instructors.FindAsync(instructorId);

            if (course == null || instructor == null)
            {
                TempData["Error"] = "Ders veya akademisyen bulunamadı!";
                return RedirectToAction(nameof(AssignCourses));
            }

            course.InstructorId = instructorId;
            course.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{course.Name} dersi {instructor.FullName}'e başarıyla atandı!";
            return RedirectToAction(nameof(AssignCourses));
        }

        [HttpPost]
        public async Task<IActionResult> UnassignCourse(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                TempData["Error"] = "Ders bulunamadı!";
                return RedirectToAction(nameof(AssignCourses));
            }

            var instructorName = course.Instructor?.FullName ?? "Bilinmeyen";
            course.InstructorId = null; // Akademisyenden ayır (null olarak ayarla)
            course.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{course.Name} dersi {instructorName}'den kaldırıldı!";
            return RedirectToAction(nameof(AssignCourses));
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