namespace DersSunumSistemi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Bir kategorinin birden fazla dersi olabilir
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}