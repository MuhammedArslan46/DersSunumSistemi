namespace DersSunumSistemi.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public InstitutionType Type { get; set; } // Fakülte, Meslek Yüksekokulu, Yüksekokul

        // Bir kurumun birden fazla fakültesi olabilir
        public List<Faculty> Faculties { get; set; } = new List<Faculty>();
    }

    public enum InstitutionType
    {
        Faculty = 1,        // Fakülte
        VocationalSchool = 2,   // Meslek Yüksekokulu
        HigherSchool = 3    // Yüksekokul
    }
}
