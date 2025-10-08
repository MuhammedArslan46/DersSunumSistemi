# Ders Sunum Sistemi - Kurulum Kılavuzu

Bu dosya, projeyi başka bir bilgisayarda çalıştırmak için gereken tüm adımları detaylı olarak açıklar.

## 📋 Ön Koşullar

### 1. .NET 8.0 SDK Kurulumu

**İndirme:** https://dotnet.microsoft.com/download/dotnet/8.0

**Kurulum Kontrolü:**
```bash
dotnet --version
# Çıktı: 8.0.x olmalı
```

### 2. SQL Server Kurulumu

**Seçenekler:**
- **SQL Server Express (Ücretsiz):** https://www.microsoft.com/sql-server/sql-server-downloads
- **SQL Server Developer Edition (Ücretsiz):** Daha fazla özellik
- **LocalDB:** Visual Studio ile birlikte gelir

**Kurulum Kontrolü:**
```bash
# SQL Server Management Studio (SSMS) ile bağlanmayı deneyin
# Server name: .\SQLEXPRESS veya localhost
```

### 3. Git (Opsiyonel)

**İndirme:** https://git-scm.com/downloads

## 🚀 Kurulum Adımları

### Adım 1: Projeyi İndirin

**Git ile:**
```bash
git clone [proje-repository-url]
cd DersProjesi
```

**Manuel olarak:**
1. Projeyi ZIP olarak indirin
2. Klasöre çıkarın
3. Terminal/CMD ile klasöre gidin

### Adım 2: SQL Server Instance'ınızı Belirleyin

SQL Server Management Studio (SSMS) açın ve server name'i kontrol edin:

**Yaygın server isimleri:**
- `.\SQLEXPRESS` (SQL Server Express)
- `localhost` veya `.` (Default instance)
- `(localdb)\MSSQLLocalDB` (LocalDB)
- `.\MSSQLSERVER` (Named instance)

### Adım 3: Connection String'i Yapılandırın

`DersSunumSistemi/appsettings.json` dosyasını açın:

**Windows Authentication için (Önerilen):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DersSunumDB;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**SQL Server Authentication için:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=DersSunumDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

**LocalDB için:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=DersSunumDB;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

> **Not:** `Server=` kısmını kendi SQL Server instance'ınıza göre değiştirin!

### Adım 4: EF Core Tools Kurulumu

Entity Framework Core tools'u global olarak yükleyin:

```bash
dotnet tool install --global dotnet-ef
```

**Zaten kuruluysa, güncelleyin:**
```bash
dotnet tool update --global dotnet-ef
```

**Kurulum kontrolü:**
```bash
dotnet ef --version
# Çıktı: 9.x.x veya üzeri olmalı
```

### Adım 5: Proje Bağımlılıklarını Yükleyin

```bash
cd DersSunumSistemi
dotnet restore
```

Bu komut tüm NuGet paketlerini indirir.

### Adım 6: Veritabanını Oluşturun

```bash
dotnet ef database update
```

**Bu komut:**
- ✅ DersSunumDB veritabanını oluşturur
- ✅ Tüm tabloları oluşturur (Users, Institutions, Faculties, Departments, Courses, Presentations, Categories, Instructors)
- ✅ Örnek verileri ekler:
  - 1 Kurum (Örnek Üniversitesi)
  - 2 Fakülte (Mühendislik, Tıp)
  - 4 Bölüm (Bilgisayar Müh., Elektrik Müh., Cerrahi, İç Hastalıkları)
  - Test kullanıcıları (admin, instructor1, student1)

### Adım 7: Projeyi Çalıştırın

```bash
dotnet run
```

**Başarılı çıktı:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5178
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Adım 8: Tarayıcıda Açın

Tarayıcınızda şu adresi açın:
```
http://localhost:5178
```

## 👤 Giriş Bilgileri

### Admin Hesabı
- **Kullanıcı Adı:** `admin`
- **Şifre:** `admin123`
- **Yetkiler:** Tüm sistem yönetimi

### Öğretim Üyesi Hesabı
- **Kullanıcı Adı:** `instructor1`
- **Şifre:** `instructor123`
- **Yetkiler:** Ders ve sunum yönetimi

### Öğrenci Hesabı
- **Kullanıcı Adı:** `student1`
- **Şifre:** `student123`
- **Yetkiler:** Kendi bölümünün derslerini görüntüleme

## ❌ Sık Karşılaşılan Hatalar ve Çözümleri

### Hata 1: "A connection was successfully established... but login failed"

**Sebep:** SQL Server'a bağlanma izni yok

**Çözüm:**
1. SQL Server Management Studio açın
2. Security → Logins → Windows kullanıcınıza sağ tıklayın
3. Properties → Server Roles → sysadmin seçin

### Hata 2: "Cannot open database 'DersSunumDB'"

**Sebep:** Veritabanı oluşturulamadı

**Çözüm:**
```bash
# Migration'ları kontrol edin
dotnet ef migrations list

# Veritabanını yeniden oluşturun
dotnet ef database drop --force
dotnet ef database update
```

### Hata 3: "The term 'dotnet' is not recognized"

**Sebep:** .NET SDK kurulu değil veya PATH'e eklenmemiş

**Çözüm:**
1. .NET SDK'yı yükleyin
2. Bilgisayarı yeniden başlatın
3. `dotnet --version` ile kontrol edin

### Hata 4: "dotnet ef: command not found"

**Sebep:** EF Core tools kurulu değil

**Çözüm:**
```bash
dotnet tool install --global dotnet-ef
```

### Hata 5: "Port 5178 already in use"

**Sebep:** Port başka bir uygulama tarafından kullanılıyor

**Çözüm 1:** Diğer uygulamayı kapatın

**Çözüm 2:** Port numarasını değiştirin
- `DersSunumSistemi/Properties/launchSettings.json` dosyasını açın
- `applicationUrl` satırındaki portu değiştirin

### Hata 6: "Could not find a part of the path '...\\uploads'"

**Sebep:** Uploads klasörü yok

**Çözüm:**
```bash
# DersSunumSistemi klasöründe
mkdir wwwroot\uploads
```

## 🔧 Geliştirme Modunda Çalıştırma

### Watch Mode (Otomatik yeniden başlatma)

```bash
dotnet watch run
```

Kod değişikliklerinde otomatik olarak yeniden derlenir ve başlatır.

### Veritabanını Sıfırlama

```bash
# Tüm verileri sil
dotnet ef database drop

# Yeniden oluştur
dotnet ef database update
```

### Yeni Migration Oluşturma

Model değişikliklerinden sonra:

```bash
dotnet ef migrations add MigrationAdiniz
dotnet ef database update
```

## 📁 Proje Klasör Yapısı

```
DersProjesi/
├── DersSunumSistemi/              # Ana proje klasörü
│   ├── Controllers/               # MVC Controllers
│   ├── Models/                    # Veri modelleri
│   ├── Views/                     # Razor view'ları
│   ├── Data/                      # DbContext ve seeding
│   ├── Services/                  # İş mantığı servisleri
│   ├── Migrations/                # EF Core migrations
│   ├── wwwroot/                   # Statik dosyalar
│   │   ├── css/                   # CSS dosyaları
│   │   ├── js/                    # JavaScript
│   │   └── uploads/               # Yüklenen sunumlar
│   ├── appsettings.json           # Yapılandırma
│   ├── Program.cs                 # Uygulama başlangıcı
│   └── DersSunumSistemi.csproj    # Proje dosyası
├── README.md                      # Proje açıklaması
└── SETUP.md                       # Bu dosya

```

## 🤝 Ekip Çalışması İçin Notlar

### Git Kullanıyorsanız

**appsettings.json'u commit etmeyin!**

`.gitignore` dosyasına ekleyin:
```
appsettings.json
appsettings.Development.json
```

Her geliştirici kendi `appsettings.json` dosyasını oluşturmalı.

### Örnek dosyayı kullanın:
```bash
copy appsettings.Example.json appsettings.json
# Sonra kendi ayarlarınızı düzenleyin
```

### Migration'ları Senkronize Tutma

```bash
# Yeni bir migration çekildiğinde
git pull
dotnet ef database update
```

## 📞 Yardım

Sorun yaşıyorsanız:
1. Bu dosyayı tekrar okuyun
2. Hata mesajını Google'da aratın
3. Proje sahibine ulaşın

**Başarılar!** 🎉
