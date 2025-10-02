namespace DersSunumSistemi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
        // Hangi kategoriye ait
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        
        // Bir dersin birden fazla sunumu olabilir
        public List<Presentation> Presentations { get; set; } = new List<Presentation>();
    }
}