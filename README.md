# Ders Sunum Sistemi

ASP.NET Core MVC ile geliştirilmiş ders sunumlarını yönetme ve paylaşma platformu.

## Özellikler

- ✅ Kategori yönetimi
- ✅ Ders yönetimi
- ✅ Sunum yükleme ve indirme
- ✅ Admin paneli
- ✅ Modern ve responsive tasarım
- ✅ Bootstrap 5 & Bootstrap Icons

## Kurulum

### 1. Gereksinimler
- .NET 8.0 SDK
- SQL Server Express
- Visual Studio 2022 veya VS Code (opsiyonel)

### 2. Projeyi Çalıştırma

```bash
# Projeyi klonlayın
git clone <repository-url>
cd DersProjesi/DersSunumSistemi

# NuGet paketlerini yükleyin
dotnet restore

# Veritabanını oluşturun (otomatik)
# appsettings.json'da connection string'i kontrol edin
# Varsayılan: Server=.\\SQLEXPRESS;Database=DersSunumDB;...

# Uygulamayı çalıştırın
dotnet run
```

### 3. Varsayılan Admin Girişi

- **Kullanıcı:** admin
- **Şifre:** admin123

## Kullanım

1. Tarayıcıdan `http://localhost:5178` adresini açın
2. **Admin Panel** butonuna tıklayın
3. Admin bilgileriyle giriş yapın
4. Kategoriler, Dersler ve Sunumlar ekleyin

## Veritabanı

Uygulama ilk çalıştığında otomatik olarak:
- Veritabanını oluşturur
- Tabloları oluşturur
- Varsayılan admin kullanıcısı ekler

## Proje Yapısı

```
DersSunumSistemi/
├── Controllers/         # MVC Controllers
├── Models/             # Data models
├── Views/              # Razor views
├── Data/               # DbContext
├── wwwroot/            # Static files
│   └── uploads/        # Yüklenen dosyalar
└── appsettings.json    # Konfigürasyon
```

## Teknolojiler

- ASP.NET Core 8.0 MVC
- Entity Framework Core 9.0
- SQL Server Express
- Bootstrap 5
- Bootstrap Icons

## Notlar

- Sunumlar `wwwroot/uploads/presentations/` klasörüne kaydedilir
- Session tabanlı authentication kullanılır
- Dosya formatları: PDF, PowerPoint, Word

## Lisans

MIT License
