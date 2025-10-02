namespace DersSunumSistemi.Models
{
    public class Presentation
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        
        // Hangi derse ait
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}