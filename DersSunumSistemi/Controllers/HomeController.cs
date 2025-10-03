using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Models;
using DersSunumSistemi.Data;

namespace DersSunumSistemi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _logger = logger;
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, int? departmentId)
    {
        var coursesQuery = _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .ThenInclude(i => i!.Department)
            .Include(c => c.Presentations)
            .Where(c => c.IsActive);

        // Arama filtresi
        if (!string.IsNullOrWhiteSpace(search))
        {
            coursesQuery = coursesQuery.Where(c =>
                c.Name.Contains(search) ||
                c.Code.Contains(search) ||
                c.Description.Contains(search) ||
                c.Instructor!.FullName.Contains(search));
        }

        // Kategori filtresi
        if (categoryId.HasValue)
        {
            coursesQuery = coursesQuery.Where(c => c.CategoryId == categoryId.Value);
        }

        // Bölüm filtresi
        if (departmentId.HasValue)
        {
            coursesQuery = coursesQuery.Where(c => c.Instructor!.DepartmentId == departmentId.Value);
        }

        var courses = await coursesQuery
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Departments = await _context.Departments.ToListAsync();
        ViewBag.Search = search;
        ViewBag.CategoryId = categoryId;
        ViewBag.DepartmentId = departmentId;

        return View(courses);
    }

    public async Task<IActionResult> Categories()
    {
        var categories = await _context.Categories
            .Include(c => c.Courses.Where(course => course.IsActive))
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Category(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Courses.Where(course => course.IsActive))
            .ThenInclude(c => c.Instructor)
            .ThenInclude(i => i!.Department)
            .Include(c => c.Courses)
            .ThenInclude(c => c.Presentations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    public async Task<IActionResult> Departments()
    {
        var departments = await _context.Departments
            .Include(d => d.Instructors)
            .ThenInclude(i => i.Courses.Where(c => c.IsActive))
            .ToListAsync();

        return View(departments);
    }

    public async Task<IActionResult> Department(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Instructors)
            .ThenInclude(i => i.Courses.Where(c => c.IsActive))
            .ThenInclude(c => c.Category)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
            return NotFound();

        return View(department);
    }

    public async Task<IActionResult> Instructors()
    {
        var instructors = await _context.Instructors
            .Include(i => i.Department)
            .Include(i => i.Courses.Where(c => c.IsActive))
            .OrderBy(i => i.Department!.Name)
            .ThenBy(i => i.FullName)
            .ToListAsync();

        return View(instructors);
    }

    public async Task<IActionResult> Instructor(int id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.Department)
            .Include(i => i.Courses.Where(c => c.IsActive))
            .ThenInclude(c => c.Category)
            .Include(i => i.Courses)
            .ThenInclude(c => c.Presentations)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            return NotFound();

        return View(instructor);
    }

    public async Task<IActionResult> Course(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .ThenInclude(i => i!.Department)
            .Include(c => c.Presentations.Where(p => p.IsPublished))
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (course == null)
            return NotFound();

        return View(course);
    }

    public async Task<IActionResult> Presentation(int id)
    {
        var presentation = await _context.Presentations
            .Include(p => p.Course)
            .ThenInclude(c => c!.Instructor)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsPublished);

        if (presentation == null)
            return NotFound();

        // Görüntülenme sayısını artır
        presentation.ViewCount++;
        await _context.SaveChangesAsync();

        return View(presentation);
    }

    public async Task<IActionResult> Download(int id)
    {
        var presentation = await _context.Presentations
            .FirstOrDefaultAsync(p => p.Id == id && p.IsPublished);

        if (presentation == null)
            return NotFound();

        // İndirme sayısını artır
        presentation.DownloadCount++;
        await _context.SaveChangesAsync();

        var filePath = Path.Combine(_environment.WebRootPath, presentation.FilePath.TrimStart('/'));

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/octet-stream", presentation.FileName);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
