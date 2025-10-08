# Ders Sunum Sistemi

## Proje Hakkında

Ders Sunum Sistemi, eğitim kurumları ve öğretim görevlileri için geliştirilmiş bir web uygulamasıdır. Bu sistem, ders materyallerinin kategorize edilmiş ve düzenli bir şekilde saklanmasını, yönetilmesini ve öğrencilere sunulmasını sağlar.

## MVC Mimarisi Nedir?

**MVC (Model-View-Controller)**, yazılım geliştirmede kullanılan bir mimari tasarım desenidir. Uygulamayı üç ana bileşene ayırarak kodun daha düzenli, sürdürülebilir ve test edilebilir olmasını sağlar:

### 1. Model (Veri Katmanı)
- **Görev**: Uygulamanın verilerini ve iş mantığını temsil eder
- **Projemizde**:
  - `User.cs` - Kullanıcı bilgilerini tutar (Id, Kullanıcı Adı, Şifre, Tam İsim, Admin yetkisi)
  - `Category.cs` - Ders kategorilerini temsil eder (Matematik, Fizik, vb.)
  - `Course.cs` - Dersleri temsil eder (İlişkili kategori, eğitmen bilgisi)
  - `Presentation.cs` - Sunum dosyalarını temsil eder (Başlık, açıklama, dosya yolu)

### 2. View (Görünüm Katmanı)
- **Görev**: Kullanıcıya gösterilecek arayüzü oluşturur
- **Projemizde**:
  - `Views/Home/Index.cshtml` - Ana sayfa, kategorileri listeler
  - `Views/Home/Category.cshtml` - Seçilen kategorideki dersleri gösterir
  - `Views/Home/Course.cshtml` - Ders detayları ve sunumları listeler
  - `Views/Admin/Login.cshtml` - Admin giriş sayfası
  - `Views/Categories/`, `Courses/`, `Presentations/` - CRUD işlemleri için sayfalar

### 3. Controller (Kontrol Katmanı)
- **Görev**: Model ve View arasında köprü görevi görür, kullanıcı isteklerini yönetir
- **Projemizde**:
  - `HomeController.cs` - Ana sayfa ve gezinme işlemlerini yönetir
  - `AdminController.cs` - Admin girişi ve yetkilendirme
  - `CategoriesController.cs` - Kategori CRUD işlemleri
  - `CoursesController.cs` - Ders CRUD işlemleri
  - `PresentationsController.cs` - Sunum yükleme ve yönetimi

## MVC'nin Avantajları

1. **Separation of Concerns (Sorumlulukların Ayrılması)**: Her katman kendi işine odaklanır
2. **Test Edilebilirlik**: Her katman bağımsız test edilebilir
3. **Yeniden Kullanılabilirlik**: Aynı Model farklı View'larda kullanılabilir
4. **Paralel Geliştirme**: Ekip üyeleri farklı katmanlarda eş zamanlı çalışabilir
5. **Bakım Kolaylığı**: Değişiklikler sadece ilgili katmanda yapılır

## Proje Yapısı ve İşleyiş

### Veritabanı Tasarımı (Entity Framework Core)

```
ApplicationDbContext.cs (Data Katmanı)
├── Users (Kullanıcılar tablosu)
├── Categories (Kategoriler tablosu)
│   └── Courses (1-N ilişki: Bir kategorinin birçok dersi var)
├── Courses (Dersler tablosu)
│   ├── CategoryId (Foreign Key)
│   └── Presentations (1-N ilişki: Bir dersin birçok sunumu var)
└── Presentations (Sunumlar tablosu)
    └── CourseId (Foreign Key)
```

### Kullanılan Teknolojiler

- **ASP.NET Core 8.0**: Web framework
- **Entity Framework Core 9.0**: ORM (Object-Relational Mapping)
- **SQL Server**: Veritabanı
- **Bootstrap 5**: UI framework
- **Razor Pages**: View engine

### Özellikler

#### Kullanıcı Tarafı
1. **Ana Sayfa**: Tüm kategorileri görüntüleme
2. **Kategori Sayfası**: Seçilen kategorideki dersleri listeleme
3. **Ders Sayfası**: Ders detayları ve sunumları görüntüleme
4. **Sunum İndirme**: Dosyaları indirme imkanı

#### Admin Paneli
1. **Güvenli Giriş**: Session tabanlı kimlik doğrulama
2. **Kategori Yönetimi**: Ekleme, düzenleme, silme (CRUD)
3. **Ders Yönetimi**: Kategorilere ders ekleme/düzenleme
4. **Sunum Yönetimi**: PDF, PowerPoint dosyalarını yükleme ve yönetme

## Nasıl Çalıştırılır?

### Gereksinimler
- .NET 8.0 SDK ([İndir](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQL Server veya SQL Server Express ([İndir](https://www.microsoft.com/sql-server/sql-server-downloads))
- Visual Studio 2022 veya VS Code (opsiyonel)

### Adımlar

#### 1. Projeyi İndirin/Klonlayın
```bash
git clone [proje-url]
cd DersProjesi
```

#### 2. Veritabanı Bağlantısını Yapılandırın

`DersSunumSistemi/appsettings.json` dosyasını açın ve connection string'i kendi SQL Server ayarlarınıza göre düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DersSunumDB;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Notlar:**
- `Server=.\\SQLEXPRESS` - SQL Server instance adınızı buraya yazın
- Farklı bir instance kullanıyorsanız (örn: `Server=localhost` veya `Server=.\MSSQLSERVER`)
- Windows Authentication yerine SQL Server Authentication kullanmak için:
  ```
  Server=.\\SQLEXPRESS;Database=DersSunumDB;User Id=sa;Password=SİFRENİZ;TrustServerCertificate=True
  ```

#### 3. Bağımlılıkları Yükleyin
```bash
cd DersSunumSistemi
dotnet restore
```

#### 4. Veritabanını Oluşturun ve Migration'ları Uygulayın
```bash
dotnet ef database update
```

Bu komut:
- DersSunumDB veritabanını otomatik oluşturur
- Tüm tabloları (Users, Institutions, Faculties, Departments, Courses, Presentations, vb.) oluşturur
- Örnek verileri ekler (kurum, fakülte, bölüm ve test kullanıcıları)

#### 5. Uygulamayı Çalıştırın
```bash
dotnet run
```

#### 6. Tarayıcıda Açın
```
http://localhost:5178
```

### Varsayılan Test Kullanıcıları

Sistem otomatik olarak aşağıdaki kullanıcıları oluşturur:

#### Admin
- **Kullanıcı Adı**: admin
- **Şifre**: admin123
- **Yetki**: Sistem yöneticisi

#### Öğretim Üyeleri
- **Kullanıcı Adı**: instructor1
- **Şifre**: instructor123
- **Yetki**: Ders ve sunum yönetimi

#### Öğrenciler
- **Kullanıcı Adı**: student1
- **Şifre**: student123
- **Yetki**: Dersleri görüntüleme (kendi bölümü)

## Projenin Teknik Detayları

### 1. Routing Yapısı
```csharp
// Program.cs içinde
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```
- `/` → Ana sayfa (Kategoriler)
- `/Home/Category/1` → 1 numaralı kategori
- `/Home/Course/1` → 1 numaralı ders
- `/Admin` → Admin paneli

### 2. Veri İlişkileri (Entity Framework)

**Include ve ThenInclude Kullanımı:**
```csharp
// HomeController.cs:22-24
var categories = await _context.Categories
    .Include(c => c.Courses)              // Kategorinin derslerini dahil et
    .ThenInclude(c => c.Presentations)    // Her dersin sunumlarını dahil et
    .ToListAsync();
```

Bu yapı sayesinde N+1 sorunu önlenir ve tek sorguda tüm ilişkili veriler çekilir.

### 3. Dosya Yükleme Sistemi

```csharp
// PresentationsController.cs içinde
public async Task<IActionResult> Create(Presentation presentation, IFormFile file)
{
    if (file != null)
    {
        var uploadsFolder = Path.Combine("wwwroot", "uploads");
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        presentation.FileName = uniqueFileName;
        presentation.FilePath = "/uploads/" + uniqueFileName;
    }
}
```

### 4. Session Yönetimi (Admin Kontrolü)

```csharp
// AdminController.cs içinde giriş kontrolü
HttpContext.Session.SetString("AdminId", user.Id.ToString());
HttpContext.Session.SetString("AdminName", user.FullName);

// Diğer controller'larda yetki kontrolü
if (HttpContext.Session.GetString("AdminId") == null)
{
    return RedirectToAction("Login", "Admin");
}
```

## Öğrendiklerim ve Geliştirme Süreci

### 1. MVC Pattern Anlayışı
- Controller'ların nasıl çalıştığını öğrendim
- Action metodlarının View'lara veri aktarımını kavradım
- Routing mekanizmasını anladım

### 2. Entity Framework Core
- Code-First yaklaşımı ile veritabanı oluşturmayı öğrendim
- Navigation properties ile ilişkisel veritabanı tasarımını uyguladım
- LINQ sorguları ile veri çekme işlemlerini gerçekleştirdim

### 3. Asenkron Programlama
- `async/await` kullanımını öğrendim
- `Task<IActionResult>` dönen metodlar yazdım
- Veritabanı işlemlerinde asenkron metodlar kullandım

### 4. Dosya İşlemleri
- `IFormFile` ile dosya yükleme
- `wwwroot` klasöründe statik dosya yönetimi
- Güvenli dosya adlandırma (GUID kullanımı)

## Karşılaşılan Sorunlar ve Çözümler

### Sorun 1: Null Reference Uyarıları
**Çözüm**: Navigation property'lerde nullable (`?`) kullanımı
```csharp
public Category? Category { get; set; }  // Nullable referans
```

### Sorun 2: Cascade Delete Problemleri
**Çözüm**: Entity Framework'ün otomatik cascade delete davranışını kullanma

### Sorun 3: Session Yönetimi
**Çözüm**: Program.cs'de session middleware'i ekleme
```csharp
builder.Services.AddSession();
app.UseSession();
```

## Sorun Giderme

### Hata: "Connection could not be established"

**Çözüm 1:** SQL Server'ın çalıştığından emin olun
```bash
# Windows'ta SQL Server servisini kontrol edin
services.msc
# "SQL Server (SQLEXPRESS)" servisinin çalıştığını kontrol edin
```

**Çözüm 2:** Connection string'i kontrol edin
- SQL Server Management Studio (SSMS) ile bağlanmayı deneyin
- Server name'i doğrulayın (`.\\SQLEXPRESS`, `localhost`, `(localdb)\\MSSQLLocalDB`, vb.)

**Çözüm 3:** SQL Server Authentication kullanıyorsanız şifreyi kontrol edin

### Hata: "A network-related or instance-specific error"

- SQL Server Browser servisinin çalıştığından emin olun
- TCP/IP protokolünün aktif olduğunu kontrol edin (SQL Server Configuration Manager)
- Firewall ayarlarını kontrol edin

### Hata: "dotnet ef komutu bulunamadı"

```bash
# EF Core tools'u global olarak yükleyin
dotnet tool install --global dotnet-ef

# Veya güncelleyin
dotnet tool update --global dotnet-ef
```

### Migration Hataları

Eğer migration'larda sorun yaşıyorsanız:

```bash
# Veritabanını sıfırlayın
dotnet ef database drop --force

# Migration'ları yeniden uygulayın
dotnet ef database update
```

### Port Çakışması (Port 5178 kullanımda)

`DersSunumSistemi/Properties/launchSettings.json` dosyasından port numarasını değiştirebilirsiniz.

## Geliştirme İçin Notlar

### Yeni Migration Oluşturma

```bash
# Model değişikliklerinden sonra
dotnet ef migrations add MigrationAdi

# Migration'ı uygula
dotnet ef database update
```

### Watch Mode ile Çalıştırma

Geliştirme sırasında otomatik yeniden başlatma için:

```bash
dotnet watch run
```

### Veritabanını Sıfırlama

```bash
# Veritabanını tamamen sil
dotnet ef database drop

# Yeniden oluştur
dotnet ef database update
```

## Geliştirme Fikirleri

- [x] Kullanıcı kayıt sistemi
- [x] Öğrenci ve öğretmen rolleri
- [x] Bölüm bazlı ders filtreleme
- [ ] Sunum dosyalarına yorum yapabilme
- [ ] Arama ve filtreleme özellikleri
- [ ] Responsive tasarım iyileştirmeleri
- [ ] API desteği (RESTful)

## İletişim

**Geliştirici**: Muhammed Arslan
**GitHub**: [MuhammedArslan46](https://github.com/MuhammedArslan46)
**Proje Linki**: [DersSunumSistemi](https://github.com/MuhammedArslan46/DersSunumSistemi)

---

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
