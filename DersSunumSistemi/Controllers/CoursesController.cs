using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;

namespace DersSunumSistemi.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // Liste
        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Auth");

            var userId = int.Parse(userIdString);
            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login", "Auth");

            IQueryable<Course> coursesQuery = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Department)
                .Include(c => c.Presentations);

            // Admin tüm dersleri görebilir, öğrenci sadece kendi bölümünün derslerini
            if (user.Role == UserRole.Student && user.DepartmentId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.DepartmentId == user.DepartmentId.Value);
            }

            var courses = await coursesQuery
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View(courses);
        }

        // Create GET
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                course.CreatedDate = DateTime.Now;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ders başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // Edit GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id != course.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Ders başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", course.DepartmentId);
            return View(course);
        }

        // Delete GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Presentations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ders başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Detay
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Presentations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
