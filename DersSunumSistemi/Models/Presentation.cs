namespace DersSunumSistemi.Models
{
    public enum PresentationType
    {
        Lecture = 1,      // Ders Anlatımı
        Exercise = 2,     // Alıştırma
        Exam = 3,         // Sınav
        Project = 4,      // Proje
        Document = 5,     // Döküman
        Video = 6,        // Video
        Other = 7         // Diğer
    }

    public class Presentation
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PresentationType Type { get; set; } = PresentationType.Lecture;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? FileType { get; set; } // pdf, pptx, mp4, vb.
        public long FileSize { get; set; } // Byte cinsinden
        public int? Week { get; set; } // Hangi hafta
        public int ViewCount { get; set; } = 0;
        public int DownloadCount { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
        public DateTime UploadDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Hangi derse ait
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}