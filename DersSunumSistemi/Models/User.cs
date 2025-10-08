namespace DersSunumSistemi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Student;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Öğrenci için bölüm bilgisi (Student ise zorunlu)
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // İlişki: Eğer Instructor ise
        public Instructor? Instructor { get; set; }
    }
}