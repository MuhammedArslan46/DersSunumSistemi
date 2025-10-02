using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;

namespace DersSunumSistemi.Controllers
{
    public class PresentationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PresentationsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // Liste
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            var presentations = await _context.Presentations
                .Include(p => p.Course)
                .ThenInclude(c => c.Category)
                .OrderByDescending(p => p.UploadDate)
                .ToListAsync();

            return View(presentations);
        }

        // Create GET
        public IActionResult Create(int? courseId)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            ViewBag.Courses = new SelectList(_context.Courses.Include(c => c.Category), "Id", "Name", courseId);

            if (courseId.HasValue)
            {
                ViewBag.SelectedCourseId = courseId.Value;
            }

            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Presentation presentation, IFormFile? file)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                // Dosya yükleme
                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "presentations");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Benzersiz dosya adı oluştur
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı kaydet
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    presentation.FileName = file.FileName;
                    presentation.FilePath = "/uploads/presentations/" + uniqueFileName;
                }

                presentation.UploadDate = DateTime.Now;
                _context.Presentations.Add(presentation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sunum başarıyla yüklendi!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = new SelectList(_context.Courses.Include(c => c.Category), "Id", "Name", presentation.CourseId);
            return View(presentation);
        }

        // Edit GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null)
                return NotFound();

            ViewBag.Courses = new SelectList(_context.Courses.Include(c => c.Category), "Id", "Name", presentation.CourseId);
            return View(presentation);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Presentation presentation, IFormFile? file)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id != presentation.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Yeni dosya yüklendiyse
                    if (file != null && file.Length > 0)
                    {
                        // Eski dosyayı sil
                        if (!string.IsNullOrEmpty(presentation.FilePath))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, presentation.FilePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Yeni dosyayı kaydet
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "presentations");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        presentation.FileName = file.FileName;
                        presentation.FilePath = "/uploads/presentations/" + uniqueFileName;
                    }

                    _context.Update(presentation);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Sunum başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentationExists(presentation.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = new SelectList(_context.Courses.Include(c => c.Category), "Id", "Name", presentation.CourseId);
            return View(presentation);
        }

        // Delete GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            if (id == null)
                return NotFound();

            var presentation = await _context.Presentations
                .Include(p => p.Course)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (presentation == null)
                return NotFound();

            return View(presentation);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Admin");

            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation != null)
            {
                // Dosyayı sil
                if (!string.IsNullOrEmpty(presentation.FilePath))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, presentation.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Presentations.Remove(presentation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sunum başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Download dosya
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
                return NotFound();

            var presentation = await _context.Presentations.FindAsync(id);
            if (presentation == null || string.IsNullOrEmpty(presentation.FilePath))
                return NotFound();

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, presentation.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", presentation.FileName);
        }

        private bool PresentationExists(int id)
        {
            return _context.Presentations.Any(e => e.Id == id);
        }
    }
}
