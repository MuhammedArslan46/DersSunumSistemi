namespace DersSunumSistemi.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Hangi kuruma ait
        public int InstitutionId { get; set; }
        public Institution? Institution { get; set; }

        // Bir fakültenin birden fazla bölümü olabilir
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
