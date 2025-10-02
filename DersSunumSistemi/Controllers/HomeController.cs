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

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Courses)
            .ThenInclude(c => c.Presentations)
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Category(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Courses)
            .ThenInclude(c => c.Presentations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    public async Task<IActionResult> Course(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Presentations)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound();

        return View(course);
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
