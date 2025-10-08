using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;
using System.Security.Claims;

namespace DersSunumSistemi.Controllers
{
    [Authorize(Policy = "InstructorOrAdmin")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public InstructorController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private async Task<Instructor?> GetCurrentInstructor()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return await _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(i => i.UserId == userId);
        }

        public async Task<IActionResult> Dashboard()
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Presentations)
                .Where(c => c.InstructorId == instructor.Id)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            ViewBag.Instructor = instructor;
            ViewBag.TotalCourses = courses.Count;
            ViewBag.TotalPresentations = courses.Sum(c => c.Presentations.Count);
            ViewBag.TotalViews = courses.SelectMany(c => c.Presentations).Sum(p => p.ViewCount);

            return View(courses);
        }

        // COURSES
        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Presentations)
                .Where(c => c.InstructorId == instructor.Id)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCourse()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course model)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            try
            {
                // Model validation
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    TempData["Error"] = "Ders adı zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.Code))
                {
                    TempData["Error"] = "Ders kodu zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(model);
                }

                if (model.CategoryId == 0)
                {
                    TempData["Error"] = "Kategori seçimi zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(model);
                }

                model.InstructorId = instructor.Id;
                model.DepartmentId = instructor.DepartmentId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;

                _context.Courses.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Ders başarıyla oluşturuldu.";
                return RedirectToAction(nameof(MyCourses));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ders oluşturulurken bir hata oluştu: " + ex.Message;
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructor.Id);

            if (course == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course model)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.InstructorId == instructor.Id);

            if (course == null) return NotFound();

            try
            {
                // Model validation
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    TempData["Error"] = "Ders adı zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(course);
                }

                if (string.IsNullOrWhiteSpace(model.Code))
                {
                    TempData["Error"] = "Ders kodu zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(course);
                }

                if (model.CategoryId == 0)
                {
                    TempData["Error"] = "Kategori seçimi zorunludur.";
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    return View(course);
                }

                course.Name = model.Name;
                course.Code = model.Code;
                course.Description = model.Description;
                course.Syllabus = model.Syllabus;
                course.Credits = model.Credits;
                course.Semester = model.Semester;
                course.Year = model.Year;
                course.CategoryId = model.CategoryId;
                course.IsActive = model.IsActive;
                course.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Ders başarıyla güncellendi.";
                return RedirectToAction(nameof(MyCourses));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ders güncellenirken bir hata oluştu: " + ex.Message;
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(course);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            try
            {
                var course = await _context.Courses
                    .Include(c => c.Presentations)
                    .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructor.Id);

                if (course == null)
                {
                    TempData["Error"] = "Ders bulunamadı.";
                    return RedirectToAction(nameof(MyCourses));
                }

                // Delete presentation files
                foreach (var presentation in course.Presentations)
                {
                    if (!string.IsNullOrEmpty(presentation.FilePath))
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, presentation.FilePath.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Ders başarıyla silindi.";
                return RedirectToAction(nameof(MyCourses));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ders silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(MyCourses));
            }
        }

        // PRESENTATIONS
        [HttpGet]
        public async Task<IActionResult> CourseDetails(int id)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Presentations)
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructor.Id);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePresentation(int courseId)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.InstructorId == instructor.Id);

            if (course == null) return NotFound();

            ViewBag.Course = course;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePresentation(int courseId, Presentation model, IFormFile? file)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.InstructorId == instructor.Id);

            if (course == null) return NotFound();

            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    TempData["Error"] = "Materyal başlığı zorunludur.";
                    ViewBag.Course = course;
                    return View(model);
                }

                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "Dosya seçimi zorunludur.";
                    ViewBag.Course = course;
                    return View(model);
                }

                // Check file size (100MB limit)
                if (file.Length > 100 * 1024 * 1024)
                {
                    TempData["Error"] = "Dosya boyutu 100MB'dan büyük olamaz.";
                    ViewBag.Course = course;
                    return View(model);
                }

                // Upload file
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "presentations");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                model.FileName = file.FileName;
                model.FilePath = $"/uploads/presentations/{uniqueFileName}";
                model.FileSize = file.Length;
                model.FileType = Path.GetExtension(file.FileName).TrimStart('.');
                model.CourseId = courseId;
                model.UploadDate = DateTime.Now;
                model.ViewCount = 0;
                model.DownloadCount = 0;

                _context.Presentations.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Materyal başarıyla yüklendi.";
                return RedirectToAction(nameof(CourseDetails), new { id = courseId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Materyal yüklenirken bir hata oluştu: " + ex.Message;
                ViewBag.Course = course;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePresentation(int id)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            try
            {
                var presentation = await _context.Presentations
                    .Include(p => p.Course)
                    .FirstOrDefaultAsync(p => p.Id == id && p.Course!.InstructorId == instructor.Id);

                if (presentation == null)
                {
                    TempData["Error"] = "Materyal bulunamadı.";
                    return RedirectToAction(nameof(Dashboard));
                }

                var courseId = presentation.CourseId;

                // Dosyayı sil
                if (!string.IsNullOrEmpty(presentation.FilePath))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, presentation.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Presentations.Remove(presentation);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Materyal başarıyla silindi.";
                return RedirectToAction(nameof(CourseDetails), new { id = courseId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Materyal silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Dashboard));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Instructor model, IFormFile? image)
        {
            var instructor = await GetCurrentInstructor();
            if (instructor == null) return NotFound();

            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "instructors");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                instructor.ImagePath = $"/uploads/instructors/{uniqueFileName}";
            }

            instructor.Phone = model.Phone;
            instructor.Title = model.Title;
            instructor.Bio = model.Bio;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }
    }
}
