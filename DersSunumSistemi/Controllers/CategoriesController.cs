using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;

namespace DersSunumSistemi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin kontrolü
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // Liste
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            var categories = await _context.Categories
                .Include(c => c.Courses)
                .ToListAsync();
            return View(categories);
        }

        // Create GET
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kategori başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Edit GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Kategori başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Delete GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var category = await _context.Categories
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kategori başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
