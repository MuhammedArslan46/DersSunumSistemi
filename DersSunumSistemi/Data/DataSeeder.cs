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

            // 1. Kurumlar (Institutions) - Fakülteler
            var fenFakultesi = new Institution { Name = "Fen Fakültesi", Code = "FF", Description = "Fen bilimleri alanında eğitim veren fakülte", Type = InstitutionType.Faculty };
            var iibfFakultesi = new Institution { Name = "İktisadi ve İdari Bilimler Fakültesi", Code = "İİBF", Description = "İktisadi ve idari bilimler alanında eğitim", Type = InstitutionType.Faculty };
            var iletisimFakultesi = new Institution { Name = "İletişim Fakültesi", Code = "İLTF", Description = "İletişim bilimleri alanında eğitim", Type = InstitutionType.Faculty };
            var muhendislikFakultesi = new Institution { Name = "Mühendislik Fakültesi", Code = "MF", Description = "Mühendislik alanında eğitim veren fakülte", Type = InstitutionType.Faculty };
            var saglikFakultesi = new Institution { Name = "Sağlık Bilimleri Fakültesi", Code = "SBF", Description = "Sağlık bilimleri alanında eğitim", Type = InstitutionType.Faculty };
            var turizmFakultesi = new Institution { Name = "Turizm Fakültesi", Code = "TF", Description = "Turizm ve otelcilik alanında eğitim", Type = InstitutionType.Faculty };

            // Meslek Yüksekokulları
            var adaletMYO = new Institution { Name = "Adalet Meslek Yüksekokulu", Code = "AMYO", Description = "Adalet alanında 2 yıllık eğitim", Type = InstitutionType.VocationalSchool };
            var saglikMYO = new Institution { Name = "Sağlık Hizmetleri Meslek Yüksekokulu", Code = "SHMYO", Description = "Sağlık hizmetleri alanında 2 yıllık eğitim", Type = InstitutionType.VocationalSchool };
            var sosyalMYO = new Institution { Name = "Sosyal Bilimler Meslek Yüksekokulu", Code = "SBMYO", Description = "Sosyal bilimler alanında 2 yıllık eğitim", Type = InstitutionType.VocationalSchool };
            var teknikMYO = new Institution { Name = "Teknik Bilimler Meslek Yüksekokulu", Code = "TBMYO", Description = "Teknik bilimler alanında 2 yıllık eğitim", Type = InstitutionType.VocationalSchool };

            // Diğer
            var lisansTamamlama = new Institution { Name = "Lisans Tamamlama", Code = "LT", Description = "Ön lisans mezunları için lisans tamamlama programları", Type = InstitutionType.Faculty };
            var ortakDersler = new Institution { Name = "Ortak Dersler Bölümü", Code = "ODB", Description = "Tüm bölümler için ortak dersler", Type = InstitutionType.Faculty };
            var dijitalEgitim = new Institution { Name = "Dijital Eğitim", Code = "DE", Description = "Dijital eğitim ve uzaktan eğitim dersleri", Type = InstitutionType.Faculty };

            context.Institutions.AddRange(fenFakultesi, iibfFakultesi, iletisimFakultesi, muhendislikFakultesi, saglikFakultesi, turizmFakultesi,
                                         adaletMYO, saglikMYO, sosyalMYO, teknikMYO, lisansTamamlama, ortakDersler, dijitalEgitim);
            await context.SaveChangesAsync();

            // 2. Fakülteler (Faculties) - Her kurum altında

            // Fen Fakültesi altında
            var matematik = new Faculty { Name = "Matematik", Code = "MAT", Description = "Matematik bölümü", InstitutionId = fenFakultesi.Id };
            var fizik = new Faculty { Name = "Fizik", Code = "FIZ", Description = "Fizik bölümü", InstitutionId = fenFakultesi.Id };
            var kimya = new Faculty { Name = "Kimya", Code = "KIM", Description = "Kimya bölümü", InstitutionId = fenFakultesi.Id };

            // İİBF altında
            var isletme = new Faculty { Name = "İşletme", Code = "ISL", Description = "İşletme bölümü", InstitutionId = iibfFakultesi.Id };
            var iktisat = new Faculty { Name = "İktisat", Code = "IKT", Description = "İktisat bölümü", InstitutionId = iibfFakultesi.Id };
            var kamuYonetimi = new Faculty { Name = "Kamu Yönetimi", Code = "KY", Description = "Kamu yönetimi bölümü", InstitutionId = iibfFakultesi.Id };

            // İletişim Fakültesi altında
            var gazetecilik = new Faculty { Name = "Gazetecilik", Code = "GAZ", Description = "Gazetecilik bölümü", InstitutionId = iletisimFakultesi.Id };
            var halklaIliskiler = new Faculty { Name = "Halkla İlişkiler", Code = "HIL", Description = "Halkla ilişkiler bölümü", InstitutionId = iletisimFakultesi.Id };

            // Mühendislik Fakültesi altında
            var bilgisayarMuh = new Faculty { Name = "Bilgisayar Mühendisliği", Code = "BM", Description = "Bilgisayar mühendisliği bölümü", InstitutionId = muhendislikFakultesi.Id };
            var elektrikMuh = new Faculty { Name = "Elektrik-Elektronik Mühendisliği", Code = "EEM", Description = "Elektrik-elektronik mühendisliği", InstitutionId = muhendislikFakultesi.Id };
            var insaatMuh = new Faculty { Name = "İnşaat Mühendisliği", Code = "INS", Description = "İnşaat mühendisliği bölümü", InstitutionId = muhendislikFakultesi.Id };
            var makineMuh = new Faculty { Name = "Makine Mühendisliği", Code = "MAK", Description = "Makine mühendisliği bölümü", InstitutionId = muhendislikFakultesi.Id };

            // Sağlık Bilimleri Fakültesi altında
            var hemsirelik = new Faculty { Name = "Hemşirelik", Code = "HEM", Description = "Hemşirelik bölümü", InstitutionId = saglikFakultesi.Id };
            var fizyoterapi = new Faculty { Name = "Fizyoterapi ve Rehabilitasyon", Code = "FTR", Description = "Fizyoterapi bölümü", InstitutionId = saglikFakultesi.Id };

            // Turizm Fakültesi altında
            var turizmIsletme = new Faculty { Name = "Turizm İşletmeciliği", Code = "TUR", Description = "Turizm işletmeciliği bölümü", InstitutionId = turizmFakultesi.Id };
            var gastronomi = new Faculty { Name = "Gastronomi ve Mutfak Sanatları", Code = "GAS", Description = "Gastronomi bölümü", InstitutionId = turizmFakultesi.Id };

            // MYO'lar altında
            var adaletProgrami = new Faculty { Name = "Adalet Programı", Code = "AD", Description = "Adalet programı", InstitutionId = adaletMYO.Id };
            var tibbiDokuman = new Faculty { Name = "Tıbbi Dokümantasyon ve Sekreterlik", Code = "TDS", Description = "Tıbbi dokümantasyon programı", InstitutionId = saglikMYO.Id };
            var ilkAcil = new Faculty { Name = "İlk ve Acil Yardım", Code = "IAY", Description = "İlk ve acil yardım programı", InstitutionId = saglikMYO.Id };
            var muhasebe = new Faculty { Name = "Muhasebe ve Vergi Uygulamaları", Code = "MVU", Description = "Muhasebe programı", InstitutionId = sosyalMYO.Id };
            var bankacilik = new Faculty { Name = "Bankacılık ve Sigortacılık", Code = "BS", Description = "Bankacılık programı", InstitutionId = sosyalMYO.Id };
            var bilgProg = new Faculty { Name = "Bilgisayar Programlama", Code = "BP", Description = "Bilgisayar programlama", InstitutionId = teknikMYO.Id };
            var elektrikProg = new Faculty { Name = "Elektrik Programı", Code = "EP", Description = "Elektrik programı", InstitutionId = teknikMYO.Id };

            // Diğerleri
            var lisansTamamlamaIsletme = new Faculty { Name = "İşletme (Lisans Tamamlama)", Code = "ISLT", Description = "Lisans tamamlama programı", InstitutionId = lisansTamamlama.Id };
            var turkDili = new Faculty { Name = "Türk Dili", Code = "TD", Description = "Türk dili dersleri", InstitutionId = ortakDersler.Id };
            var yabanciDil = new Faculty { Name = "Yabancı Dil", Code = "YD", Description = "Yabancı dil dersleri", InstitutionId = ortakDersler.Id };
            var dijitalOkuryazar = new Faculty { Name = "Dijital Okuryazarlık", Code = "DO", Description = "Dijital okuryazarlık dersleri", InstitutionId = dijitalEgitim.Id };
            var uzaktanEgitim = new Faculty { Name = "Uzaktan Eğitim Sistemleri", Code = "UES", Description = "Uzaktan eğitim dersleri", InstitutionId = dijitalEgitim.Id };

            context.Faculties.AddRange(matematik, fizik, kimya, isletme, iktisat, kamuYonetimi, gazetecilik, halklaIliskiler,
                                      bilgisayarMuh, elektrikMuh, insaatMuh, makineMuh, hemsirelik, fizyoterapi,
                                      turizmIsletme, gastronomi, adaletProgrami, tibbiDokuman, ilkAcil, muhasebe,
                                      bankacilik, bilgProg, elektrikProg, lisansTamamlamaIsletme, turkDili, yabanciDil,
                                      dijitalOkuryazar, uzaktanEgitim);
            await context.SaveChangesAsync();

            // 3. Bölümler (Departments) - Her fakülte altında alt bölümler

            // Matematik altında
            var matematikBolumu = new Department { Name = "Matematik Bölümü", Code = "MAT-001", Description = "Matematik", FacultyId = matematik.Id };

            // Fizik altında
            var fizikBolumu = new Department { Name = "Fizik Bölümü", Code = "FIZ-001", Description = "Fizik", FacultyId = fizik.Id };

            // Kimya altında
            var kimyaBolumu = new Department { Name = "Kimya Bölümü", Code = "KIM-001", Description = "Kimya", FacultyId = kimya.Id };

            // İşletme altında
            var isletmeBolumu = new Department { Name = "İşletme Bölümü", Code = "ISL-001", Description = "İşletme", FacultyId = isletme.Id };

            // İktisat altında
            var iktisatBolumu = new Department { Name = "İktisat Bölümü", Code = "IKT-001", Description = "İktisat", FacultyId = iktisat.Id };

            // Kamu Yönetimi altında
            var kamuYonetimiBolumu = new Department { Name = "Kamu Yönetimi Bölümü", Code = "KY-001", Description = "Kamu yönetimi", FacultyId = kamuYonetimi.Id };

            // Gazetecilik altında
            var gazetecilikBolumu = new Department { Name = "Gazetecilik Bölümü", Code = "GAZ-001", Description = "Gazetecilik", FacultyId = gazetecilik.Id };

            // Halkla İlişkiler altında
            var halklaIliskilerBolumu = new Department { Name = "Halkla İlişkiler Bölümü", Code = "HIL-001", Description = "Halkla ilişkiler", FacultyId = halklaIliskiler.Id };

            // Bilgisayar Mühendisliği altında
            var yazilimMuh = new Department { Name = "Yazılım Mühendisliği", Code = "YM-001", Description = "Yazılım mühendisliği", FacultyId = bilgisayarMuh.Id };
            var ybs = new Department { Name = "Yönetim Bilişim Sistemleri", Code = "YBS-001", Description = "Yönetim bilişim sistemleri", FacultyId = bilgisayarMuh.Id };

            // Elektrik-Elektronik altında
            var elektronikBolumu = new Department { Name = "Elektronik Mühendisliği", Code = "EM-001", Description = "Elektronik mühendisliği", FacultyId = elektrikMuh.Id };

            // İnşaat Mühendisliği altında
            var yapiMuh = new Department { Name = "Yapı Mühendisliği", Code = "YM-INS", Description = "Yapı mühendisliği", FacultyId = insaatMuh.Id };

            // Makine Mühendisliği altında
            var makineBolumu = new Department { Name = "Makine Mühendisliği Bölümü", Code = "MAK-001", Description = "Makine mühendisliği", FacultyId = makineMuh.Id };

            // Hemşirelik altında
            var hemsirelikBolumu = new Department { Name = "Hemşirelik Bölümü", Code = "HEM-001", Description = "Hemşirelik", FacultyId = hemsirelik.Id };

            // Fizyoterapi altında
            var fizyoterapiBolumu = new Department { Name = "Fizyoterapi Bölümü", Code = "FTR-001", Description = "Fizyoterapi", FacultyId = fizyoterapi.Id };

            // Turizm altında
            var turizmBolumu = new Department { Name = "Turizm İşletmeciliği Bölümü", Code = "TUR-001", Description = "Turizm işletmeciliği", FacultyId = turizmIsletme.Id };

            // Gastronomi altında
            var gastronomiBolumu = new Department { Name = "Gastronomi Bölümü", Code = "GAS-001", Description = "Gastronomi", FacultyId = gastronomi.Id };

            // MYO bölümleri
            var adaletBolumu = new Department { Name = "Adalet", Code = "AD-001", Description = "Adalet", FacultyId = adaletProgrami.Id };
            var tibbiDokumanBolumu = new Department { Name = "Tıbbi Dokümantasyon", Code = "TDS-001", Description = "Tıbbi dokümantasyon", FacultyId = tibbiDokuman.Id };
            var ilkAcilBolumu = new Department { Name = "İlk ve Acil Yardım", Code = "IAY-001", Description = "İlk ve acil yardım", FacultyId = ilkAcil.Id };
            var muhasebeBolumu = new Department { Name = "Muhasebe", Code = "MVU-001", Description = "Muhasebe", FacultyId = muhasebe.Id };
            var bankacilikBolumu = new Department { Name = "Bankacılık", Code = "BS-001", Description = "Bankacılık", FacultyId = bankacilik.Id };
            var bilgProgBolumu = new Department { Name = "Bilgisayar Programlama", Code = "BP-001", Description = "Bilgisayar programlama", FacultyId = bilgProg.Id };
            var elektrikProgBolumu = new Department { Name = "Elektrik", Code = "EP-001", Description = "Elektrik", FacultyId = elektrikProg.Id };
            var lisansTamBolumu = new Department { Name = "İşletme", Code = "ISLT-001", Description = "İşletme lisans tamamlama", FacultyId = lisansTamamlamaIsletme.Id };
            var turkDiliBolumu = new Department { Name = "Türk Dili", Code = "TD-001", Description = "Türk dili", FacultyId = turkDili.Id };
            var yabanciDilBolumu = new Department { Name = "Yabancı Dil", Code = "YD-001", Description = "Yabancı dil", FacultyId = yabanciDil.Id };
            var dijitalOkuryazarBolumu = new Department { Name = "Dijital Okuryazarlık", Code = "DO-001", Description = "Dijital okuryazarlık", FacultyId = dijitalOkuryazar.Id };
            var uzaktanEgitimBolumu = new Department { Name = "Uzaktan Eğitim", Code = "UES-001", Description = "Uzaktan eğitim", FacultyId = uzaktanEgitim.Id };

            context.Departments.AddRange(matematikBolumu, fizikBolumu, kimyaBolumu, isletmeBolumu, iktisatBolumu, kamuYonetimiBolumu,
                                        gazetecilikBolumu, halklaIliskilerBolumu, yazilimMuh, ybs, elektronikBolumu, yapiMuh, makineBolumu,
                                        hemsirelikBolumu, fizyoterapiBolumu, turizmBolumu, gastronomiBolumu, adaletBolumu, tibbiDokumanBolumu,
                                        ilkAcilBolumu, muhasebeBolumu, bankacilikBolumu, bilgProgBolumu, elektrikProgBolumu, lisansTamBolumu,
                                        turkDiliBolumu, yabanciDilBolumu, dijitalOkuryazarBolumu, uzaktanEgitimBolumu);
            await context.SaveChangesAsync();

            // 4. Kategoriler
            var kategori1 = new Category { Name = "Temel Bilimler", Description = "Temel bilim dersleri" };
            var kategori2 = new Category { Name = "Mühendislik", Description = "Mühendislik dersleri" };
            var kategori3 = new Category { Name = "Sosyal Bilimler", Description = "Sosyal bilimler dersleri" };
            var kategori4 = new Category { Name = "Sağlık Bilimleri", Description = "Sağlık bilimleri dersleri" };

            context.Categories.AddRange(kategori1, kategori2, kategori3, kategori4);
            await context.SaveChangesAsync();

            // 5. Admin kullanıcı
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

            // 6. Örnek öğretim görevlileri
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
                DepartmentId = yazilimMuh.Id,
                UserId = user1.Id,
                CreatedDate = DateTime.Now,
                IsActive = true,
                StartDate = new DateTime(2020, 9, 1) // 1 Eylül 2020'de işe başladı
            };

            context.Instructors.Add(instructor1);
            await context.SaveChangesAsync();

            // 7. Örnek ders
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
                DepartmentId = yazilimMuh.Id,
                InstructorId = instructor1.Id
            };

            context.Courses.Add(course1);
            await context.SaveChangesAsync();
        }
    }
}
