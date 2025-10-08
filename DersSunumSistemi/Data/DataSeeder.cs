using DersSunumSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace DersSunumSistemi.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Eğer zaten veri varsa seed yapma
            if (await context.Institutions.AnyAsync())
                return;

            // 1. Kurumlar (Institutions)
            var muhendislikFakultesi = new Institution
            {
                Name = "Mühendislik Fakültesi",
                Code = "MF",
                Description = "Mühendislik alanında eğitim veren fakülte",
                Type = InstitutionType.Faculty
            };

            var tipFakultesi = new Institution
            {
                Name = "Tıp Fakültesi",
                Code = "TF",
                Description = "Sağlık bilimleri alanında eğitim veren fakülte",
                Type = InstitutionType.Faculty
            };

            var ziraatFakultesi = new Institution
            {
                Name = "Ziraat Fakültesi",
                Code = "ZF",
                Description = "Tarım ve hayvancılık alanında eğitim veren fakülte",
                Type = InstitutionType.Faculty
            };

            var meslekYuksekokulu = new Institution
            {
                Name = "Teknik Bilimler Meslek Yüksekokulu",
                Code = "TBMYO",
                Description = "Teknik alanlarda 2 yıllık önlisans eğitimi veren meslek yüksekokulu",
                Type = InstitutionType.VocationalSchool
            };

            context.Institutions.AddRange(muhendislikFakultesi, tipFakultesi, ziraatFakultesi, meslekYuksekokulu);
            await context.SaveChangesAsync();

            // 2. Fakülteler (Faculties)
            // Mühendislik Fakültesi altında
            var bilgisayarFakultesi = new Faculty
            {
                Name = "Bilgisayar Mühendisliği",
                Code = "BM",
                Description = "Bilgisayar ve yazılım mühendisliği eğitimi",
                InstitutionId = muhendislikFakultesi.Id
            };

            var insaatFakultesi = new Faculty
            {
                Name = "İnşaat Mühendisliği",
                Code = "IM",
                Description = "İnşaat ve yapı mühendisliği eğitimi",
                InstitutionId = muhendislikFakultesi.Id
            };

            var elektrikFakultesi = new Faculty
            {
                Name = "Elektrik-Elektronik Mühendisliği",
                Code = "EEM",
                Description = "Elektrik ve elektronik mühendisliği eğitimi",
                InstitutionId = muhendislikFakultesi.Id
            };

            // Tıp Fakültesi altında
            var dahiliyeFakultesi = new Faculty
            {
                Name = "Dahiliye",
                Code = "DAH",
                Description = "İç hastalıkları uzmanlık eğitimi",
                InstitutionId = tipFakultesi.Id
            };

            var cerrahiFakultesi = new Faculty
            {
                Name = "Cerrahi",
                Code = "CER",
                Description = "Genel cerrahi uzmanlık eğitimi",
                InstitutionId = tipFakultesi.Id
            };

            // Ziraat Fakültesi altında
            var ziraatMuhFakultesi = new Faculty
            {
                Name = "Ziraat Mühendisliği",
                Code = "ZM",
                Description = "Tarımsal üretim ve yönetim eğitimi",
                InstitutionId = ziraatFakultesi.Id
            };

            var veterinerFakultesi = new Faculty
            {
                Name = "Veterinerlik",
                Code = "VET",
                Description = "Hayvan sağlığı ve hastalıkları eğitimi",
                InstitutionId = ziraatFakultesi.Id
            };

            // MYO altında
            var bilgisayarProgramlamaMYO = new Faculty
            {
                Name = "Bilgisayar Programlama",
                Code = "BP",
                Description = "2 yıllık bilgisayar programlama eğitimi",
                InstitutionId = meslekYuksekokulu.Id
            };

            context.Faculties.AddRange(bilgisayarFakultesi, insaatFakultesi, elektrikFakultesi,
                dahiliyeFakultesi, cerrahiFakultesi, ziraatMuhFakultesi, veterinerFakultesi, bilgisayarProgramlamaMYO);
            await context.SaveChangesAsync();

            // 3. Bölümler (Departments)
            // Bilgisayar Mühendisliği altında
            var yazilimBolumu = new Department
            {
                Name = "Yazılım Mühendisliği Bölümü",
                Code = "YM",
                Description = "Yazılım geliştirme ve mühendisliği",
                FacultyId = bilgisayarFakultesi.Id
            };

            var ybs = new Department
            {
                Name = "Yönetim Bilişim Sistemleri",
                Code = "YBS",
                Description = "İşletme ve bilişim sistemleri",
                FacultyId = bilgisayarFakultesi.Id
            };

            // İnşaat Mühendisliği altında
            var yapiMuh = new Department
            {
                Name = "Yapı Mühendisliği",
                Code = "YM-INS",
                Description = "Bina ve yapı tasarımı",
                FacultyId = insaatFakultesi.Id
            };

            // Elektrik-Elektronik altında
            var elektronikBolumu = new Department
            {
                Name = "Elektronik Mühendisliği",
                Code = "EM",
                Description = "Elektronik sistemler ve devreler",
                FacultyId = elektrikFakultesi.Id
            };

            // Dahiliye altında
            var icHastaliklari = new Department
            {
                Name = "İç Hastalıkları",
                Code = "ICH",
                Description = "İç hastalıkları teşhis ve tedavi",
                FacultyId = dahiliyeFakultesi.Id
            };

            // Cerrahi altında
            var genelCerrahi = new Department
            {
                Name = "Genel Cerrahi",
                Code = "GC",
                Description = "Cerrahi müdahaleler ve operasyonlar",
                FacultyId = cerrahiFakultesi.Id
            };

            // Ziraat altında
            var tarimsal = new Department
            {
                Name = "Tarımsal Yapılar ve Sulama",
                Code = "TYS",
                Description = "Tarımsal altyapı ve sulama sistemleri",
                FacultyId = ziraatMuhFakultesi.Id
            };

            // Veteriner altında
            var hayvanSagligi = new Department
            {
                Name = "Hayvan Sağlığı ve Hastalıkları",
                Code = "HSH",
                Description = "Veteriner klinik ve tedavi",
                FacultyId = veterinerFakultesi.Id
            };

            // MYO altında
            var webProgramlama = new Department
            {
                Name = "Web Programlama",
                Code = "WP",
                Description = "Web teknolojileri ve programlama",
                FacultyId = bilgisayarProgramlamaMYO.Id
            };

            context.Departments.AddRange(yazilimBolumu, ybs, yapiMuh, elektronikBolumu,
                icHastaliklari, genelCerrahi, tarimsal, hayvanSagligi, webProgramlama);
            await context.SaveChangesAsync();

            // 4. Kategoriler
            var kategori1 = new Category { Name = "Temel Bilimler", Description = "Temel bilim dersleri" };
            var kategori2 = new Category { Name = "Mühendislik", Description = "Mühendislik dersleri" };
            var kategori3 = new Category { Name = "Tıp Bilimleri", Description = "Tıbbi dersler" };
            var kategori4 = new Category { Name = "Tarım", Description = "Tarım ve hayvancılık dersleri" };

            context.Categories.AddRange(kategori1, kategori2, kategori3, kategori4);
            await context.SaveChangesAsync();

            // 5. Kullanıcılar ve Akademisyenler
            // Admin kullanıcı
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "Admin User",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // Öğrenci kullanıcı
            var studentUser = new User
            {
                UserName = "mehmet.kaya",
                Email = "mehmet.kaya@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Mehmet Kaya",
                Role = UserRole.Student,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(studentUser);
            await context.SaveChangesAsync();

            // Örnek Öğrenciler (Farklı Bölümlerden)

            // 1. Yazılım Mühendisliği Öğrencisi
            var student1 = new User
            {
                UserName = "ali.yilmaz",
                Email = "ali.yilmaz@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Ali Yılmaz",
                Role = UserRole.Student,
                DepartmentId = yazilimBolumu.Id,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // 2. Tıp Öğrencisi (İç Hastalıkları)
            var student2 = new User
            {
                UserName = "ayse.demir",
                Email = "ayse.demir@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Ayşe Demir",
                Role = UserRole.Student,
                DepartmentId = icHastaliklari.Id,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // 3. Genel Cerrahi Öğrencisi
            var student3 = new User
            {
                UserName = "mehmet.kara",
                Email = "mehmet.kara@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Mehmet Kara",
                Role = UserRole.Student,
                DepartmentId = genelCerrahi.Id,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // 4. YBS Öğrencisi
            var student4 = new User
            {
                UserName = "zeynep.sahin",
                Email = "zeynep.sahin@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Zeynep Şahin",
                Role = UserRole.Student,
                DepartmentId = ybs.Id,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // 5. İnşaat Mühendisliği Öğrencisi
            var student5 = new User
            {
                UserName = "can.ozturk",
                Email = "can.ozturk@student.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
                FullName = "Can Öztürk",
                Role = UserRole.Student,
                DepartmentId = yapiMuh.Id,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.AddRange(student1, student2, student3, student4, student5);
            await context.SaveChangesAsync();

            // Yazılım Mühendisliği Hocaları
            var user1 = new User
            {
                UserName = "ahmet.yilmaz",
                Email = "ahmet.yilmaz@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Prof. Dr. Ahmet Yılmaz",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user1);
            await context.SaveChangesAsync();

            var instructor1 = new Instructor
            {
                FullName = "Prof. Dr. Ahmet Yılmaz",
                Email = "ahmet.yilmaz@university.edu.tr",
                Phone = "+90 555 111 1111",
                Title = "Prof. Dr.",
                Bio = "Yazılım mühendisliği alanında 20 yıllık deneyim",
                DepartmentId = yazilimBolumu.Id,
                UserId = user1.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor1);
            await context.SaveChangesAsync();

            // YBS Hocası
            var user2 = new User
            {
                UserName = "ayse.kaya",
                Email = "ayse.kaya@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Doç. Dr. Ayşe Kaya",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user2);
            await context.SaveChangesAsync();

            var instructor2 = new Instructor
            {
                FullName = "Doç. Dr. Ayşe Kaya",
                Email = "ayse.kaya@university.edu.tr",
                Phone = "+90 555 222 2222",
                Title = "Doç. Dr.",
                Bio = "Bilişim sistemleri ve yönetim alanında uzman",
                DepartmentId = ybs.Id,
                UserId = user2.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor2);
            await context.SaveChangesAsync();

            // İnşaat Hocası
            var user3 = new User
            {
                UserName = "mehmet.demir",
                Email = "mehmet.demir@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Dr. Öğr. Üyesi Mehmet Demir",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user3);
            await context.SaveChangesAsync();

            var instructor3 = new Instructor
            {
                FullName = "Dr. Öğr. Üyesi Mehmet Demir",
                Email = "mehmet.demir@university.edu.tr",
                Phone = "+90 555 333 3333",
                Title = "Dr. Öğr. Üyesi",
                Bio = "Yapı mühendisliği ve statik hesaplamalar",
                DepartmentId = yapiMuh.Id,
                UserId = user3.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor3);
            await context.SaveChangesAsync();

            // Elektronik Hocası
            var user4 = new User
            {
                UserName = "fatma.ozturk",
                Email = "fatma.ozturk@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Prof. Dr. Fatma Öztürk",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user4);
            await context.SaveChangesAsync();

            var instructor4 = new Instructor
            {
                FullName = "Prof. Dr. Fatma Öztürk",
                Email = "fatma.ozturk@university.edu.tr",
                Phone = "+90 555 444 4444",
                Title = "Prof. Dr.",
                Bio = "Elektronik devre tasarımı ve gömülü sistemler",
                DepartmentId = elektronikBolumu.Id,
                UserId = user4.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor4);
            await context.SaveChangesAsync();

            // Tıp Hocası
            var user5 = new User
            {
                UserName = "ali.celik",
                Email = "ali.celik@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Prof. Dr. Ali Çelik",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user5);
            await context.SaveChangesAsync();

            var instructor5 = new Instructor
            {
                FullName = "Prof. Dr. Ali Çelik",
                Email = "ali.celik@university.edu.tr",
                Phone = "+90 555 555 5555",
                Title = "Prof. Dr.",
                Bio = "İç hastalıkları ve kardiyoloji uzmanı",
                DepartmentId = icHastaliklari.Id,
                UserId = user5.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor5);
            await context.SaveChangesAsync();

            // Cerrahi Hocası
            var user6 = new User
            {
                UserName = "zeynep.arslan",
                Email = "zeynep.arslan@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Doç. Dr. Zeynep Arslan",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user6);
            await context.SaveChangesAsync();

            var instructor6 = new Instructor
            {
                FullName = "Doç. Dr. Zeynep Arslan",
                Email = "zeynep.arslan@university.edu.tr",
                Phone = "+90 555 666 6666",
                Title = "Doç. Dr.",
                Bio = "Genel cerrahi ve minimal invaziv cerrahi",
                DepartmentId = genelCerrahi.Id,
                UserId = user6.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor6);
            await context.SaveChangesAsync();

            // Ziraat Hocası
            var user7 = new User
            {
                UserName = "mustafa.koc",
                Email = "mustafa.koc@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Dr. Öğr. Üyesi Mustafa Koç",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user7);
            await context.SaveChangesAsync();

            var instructor7 = new Instructor
            {
                FullName = "Dr. Öğr. Üyesi Mustafa Koç",
                Email = "mustafa.koc@university.edu.tr",
                Phone = "+90 555 777 7777",
                Title = "Dr. Öğr. Üyesi",
                Bio = "Tarımsal sulama sistemleri ve arazi düzenlemesi",
                DepartmentId = tarimsal.Id,
                UserId = user7.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor7);
            await context.SaveChangesAsync();

            // Veteriner Hocası
            var user8 = new User
            {
                UserName = "elif.yildiz",
                Email = "elif.yildiz@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Prof. Dr. Elif Yıldız",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user8);
            await context.SaveChangesAsync();

            var instructor8 = new Instructor
            {
                FullName = "Prof. Dr. Elif Yıldız",
                Email = "elif.yildiz@university.edu.tr",
                Phone = "+90 555 888 8888",
                Title = "Prof. Dr.",
                Bio = "Veteriner klinik ve hayvan hastalıkları uzmanı",
                DepartmentId = hayvanSagligi.Id,
                UserId = user8.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor8);
            await context.SaveChangesAsync();

            // MYO Hocası
            var user9 = new User
            {
                UserName = "can.sahin",
                Email = "can.sahin@university.edu.tr",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                FullName = "Öğr. Gör. Can Şahin",
                Role = UserRole.Instructor,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.Users.Add(user9);
            await context.SaveChangesAsync();

            var instructor9 = new Instructor
            {
                FullName = "Öğr. Gör. Can Şahin",
                Email = "can.sahin@university.edu.tr",
                Phone = "+90 555 999 9999",
                Title = "Öğr. Gör.",
                Bio = "Web teknolojileri ve frontend geliştirme",
                DepartmentId = webProgramlama.Id,
                UserId = user9.Id,
                CreatedDate = DateTime.Now
            };

            context.Instructors.Add(instructor9);
            await context.SaveChangesAsync();

            // 6. Dersler
            var course1 = new Course
            {
                Name = "Veri Yapıları ve Algoritmalar",
                Code = "BIL202",
                Description = "Temel veri yapıları ve algoritma analizi",
                Syllabus = "Array, Linked List, Stack, Queue, Tree, Graph, Sorting, Searching",
                Credits = 4,
                Semester = "Güz",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori2.Id,
                DepartmentId = yazilimBolumu.Id,
                InstructorId = instructor1.Id
            };

            var course2 = new Course
            {
                Name = "Veritabanı Yönetim Sistemleri",
                Code = "YBS301",
                Description = "İlişkisel veritabanı tasarımı ve SQL",
                Syllabus = "ER Diagram, Normalizasyon, SQL, Transaction, Index",
                Credits = 3,
                Semester = "Bahar",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori2.Id,
                DepartmentId = ybs.Id,
                InstructorId = instructor2.Id
            };

            var course3 = new Course
            {
                Name = "Statik ve Mukavemet",
                Code = "INS201",
                Description = "Yapılarda statik hesaplamalar ve mukavemet analizi",
                Syllabus = "Kuvvet, Moment, Gerilme, Gerinim, Mukavemet",
                Credits = 4,
                Semester = "Güz",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori2.Id,
                DepartmentId = yapiMuh.Id,
                InstructorId = instructor3.Id
            };

            var course4 = new Course
            {
                Name = "Elektronik Devre Tasarımı",
                Code = "ELK301",
                Description = "Analog ve dijital devre tasarımı",
                Syllabus = "Transistör, Op-Amp, Dijital Kapılar, PCB Tasarım",
                Credits = 3,
                Semester = "Bahar",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori2.Id,
                DepartmentId = elektronikBolumu.Id,
                InstructorId = instructor4.Id
            };

            var course5 = new Course
            {
                Name = "İç Hastalıkları Klinik Uygulamalar",
                Code = "TIP401",
                Description = "İç hastalıkları tanı ve tedavi yöntemleri",
                Syllabus = "Kardiyoloji, Endokrinoloji, Gastroenteroloji",
                Credits = 5,
                Semester = "Güz",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori3.Id,
                DepartmentId = icHastaliklari.Id,
                InstructorId = instructor5.Id
            };

            var course6 = new Course
            {
                Name = "Cerrahi Teknikler",
                Code = "CER301",
                Description = "Temel cerrahi teknikler ve uygulamalar",
                Syllabus = "Sterilizasyon, Dikiş Teknikleri, Minimal İnvaziv Cerrahi",
                Credits = 4,
                Semester = "Bahar",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori3.Id,
                DepartmentId = genelCerrahi.Id,
                InstructorId = instructor6.Id
            };

            var course7 = new Course
            {
                Name = "Sulama Sistemleri",
                Code = "ZIR201",
                Description = "Tarımsal sulama sistemleri tasarımı",
                Syllabus = "Damla Sulama, Yağmurlama, Drenaj Sistemleri",
                Credits = 3,
                Semester = "Güz",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori4.Id,
                DepartmentId = tarimsal.Id,
                InstructorId = instructor7.Id
            };

            var course8 = new Course
            {
                Name = "Veteriner Klinik Uygulamaları",
                Code = "VET301",
                Description = "Hayvan hastalıkları teşhis ve tedavi",
                Syllabus = "Büyükbaş, Küçükbaş, Kanatlı Hayvan Hastalıkları",
                Credits = 4,
                Semester = "Bahar",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori4.Id,
                DepartmentId = hayvanSagligi.Id,
                InstructorId = instructor8.Id
            };

            var course9 = new Course
            {
                Name = "Web Programlama Temelleri",
                Code = "WEB101",
                Description = "HTML, CSS, JavaScript ile web geliştirme",
                Syllabus = "HTML5, CSS3, JavaScript, Bootstrap, Responsive Design",
                Credits = 3,
                Semester = "Güz",
                Year = 2024,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CategoryId = kategori2.Id,
                DepartmentId = webProgramlama.Id,
                InstructorId = instructor9.Id
            };

            context.Courses.AddRange(course1, course2, course3, course4, course5, course6, course7, course8, course9);
            await context.SaveChangesAsync();
        }
    }
}
