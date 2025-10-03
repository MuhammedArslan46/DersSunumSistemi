namespace DersSunumSistemi.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Title { get; set; } // Prof. Dr., Doç. Dr., Dr. Öğr. Üyesi, vb.
        public string? Bio { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }

        // Hangi bölüme ait
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Kullanıcı bilgisi
        public int UserId { get; set; }
        public User? User { get; set; }

        // Bir akademisyenin birden fazla dersi olabilir
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
