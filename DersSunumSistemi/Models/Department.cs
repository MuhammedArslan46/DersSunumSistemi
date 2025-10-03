namespace DersSunumSistemi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Bir bölümün birden fazla akademisyeni olabilir
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
}
