namespace DersSunumSistemi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // Ders kodu: BİL101, MAT201, vb.
        public string Description { get; set; } = string.Empty;
        public string? Syllabus { get; set; } // Ders içeriği
        public int Credits { get; set; } = 3; // Kredi
        public string? Semester { get; set; } // Güz, Bahar
        public int? Year { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Hangi kategoriye ait
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Hangi akademisyene ait (nullable - ders henüz atanmamış olabilir)
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        // Hangi bölüme ait
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Bir dersin birden fazla sunumu olabilir
        public List<Presentation> Presentations { get; set; } = new List<Presentation>();
    }
}