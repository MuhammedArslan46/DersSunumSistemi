namespace DersSunumSistemi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Hangi fakülteye ait
        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        // Bir bölümün birden fazla akademisyeni olabilir
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        // Bir bölümün birden fazla dersi olabilir
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
