# MTÜ Ders Sunumu Sistemi - Kurulum Kılavuzu

## Gereksinimler

Projeyi çalıştırmadan önce aşağıdaki yazılımların bilgisayarınızda kurulu olması gerekmektedir:

### 1. .NET 8.0 SDK
- **İndirme Linki**: https://dotnet.microsoft.com/download/dotnet/8.0
- .NET 8.0 SDK'yı indirip kurun
- Kurulum sonrası terminal/komut istemcisinde kontrol edin:
  ```bash
  dotnet --version
  ```
  Çıktı: `8.0.x` olmalı

### 2. SQL Server LocalDB (Önerilen) veya SQL Server
- **LocalDB** (Geliştirme için önerilen):
  - Visual Studio ile birlikte gelir
  - Veya SQL Server Express'in parçasıdır
  - İndirme: https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

- **SQL Server Express** (Alternatif):
  - İndirme: https://www.microsoft.com/sql-server/sql-server-downloads
  - "Express" sürümünü seçin (ücretsiz)

### 3. Git (Proje transferi için)
- İndirme: https://git-scm.com/downloads

### 4. Kod Editörü (Opsiyonel)
- **Visual Studio Code** (Önerilen - hafif): https://code.visualstudio.com/
- **Visual Studio 2022 Community** (Tam özellikli): https://visualstudio.microsoft.com/

---

## Kurulum Adımları

### Adım 1: Projeyi Kopyalama

#### Yöntem A: USB veya Paylaşılan Klasör
1. Proje klasörünü (`DersProjesi`) bilgisayarınıza kopyalayın
2. Örnek konum: `C:\Users\[KullaniciAdi]\Desktop\DersProjesi`

#### Yöntem B: Git ile (eğer repository varsa)
```bash
git clone [repository-url]
cd DersProjesi
```

---

### Adım 2: Veritabanı Bağlantı Ayarları

1. Proje klasöründe `DersSunumSistemi/appsettings.json` dosyasını açın

2. `ConnectionStrings` bölümünü kontrol edin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DersSunumDB;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

#### LocalDB Kullanıyorsanız (Önerilen):
- Yukarıdaki ayar olduğu gibi kalabilir
- LocalDB otomatik olarak veritabanını oluşturacak

#### SQL Server Express Kullanıyorsanız:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DersSunumDB;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

#### Farklı SQL Server Instance Kullanıyorsanız:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=[SERVER_ADI];Database=DersSunumDB;User Id=[KULLANICI];Password=[SIFRE];TrustServerCertificate=true"
  }
}
```

---

### Adım 3: Terminali/Komut İstemcisini Açma

**Windows için:**
1. Proje klasörüne gidin: `DersProjesi\DersSunumSistemi`
2. Klasör içinde boş bir alanda `Shift + Sağ Tık` yapın
3. "Terminalde Aç" veya "PowerShell penceresini burada aç" seçin

**Alternatif:**
1. `Windows + R` tuşlarına basın
2. `cmd` yazıp Enter'a basın
3. Şu komutu çalıştırın:
   ```bash
   cd C:\Users\[KullaniciAdi]\Desktop\DersProjesi\DersSunumSistemi
   ```

---

### Adım 4: Bağımlılıkları Yükleme

Terminal/Komut istemcisinde sırayla:

```bash
# 1. NuGet paketlerini geri yükle
dotnet restore

# 2. Projeyi derle
dotnet build
```

**Beklenen Çıktı:**
```
Oluşturma başarılı oldu.
    5 Uyarı
    0 Hata
```

---

### Adım 5: Veritabanını Oluşturma

#### Otomatik Oluşturma (Program.cs'de zaten var):
Proje ilk çalıştırıldığında veritabanı otomatik oluşacak ve örnek veriler eklenecek.

#### Manuel Oluşturma (Opsiyonel):
```bash
# Migration'ları kontrol et
dotnet ef migrations list

# Eğer migration yoksa, yeni migration oluştur
dotnet ef migrations add InitialCreate

# Veritabanını güncelle
dotnet ef database update
```

**Not:** `dotnet ef` komutu çalışmıyorsa:
```bash
dotnet tool install --global dotnet-ef
```

---

### Adım 6: Projeyi Çalıştırma

```bash
dotnet run
```

**Beklenen Çıktı:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5178
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

---

### Adım 7: Tarayıcıda Açma

1. Web tarayıcınızı açın (Chrome, Edge, Firefox vb.)
2. Adres çubuğuna yazın: `http://localhost:5178`
3. Enter'a basın

---

## İlk Giriş Bilgileri

Proje ilk çalıştırıldığında otomatik olarak örnek kullanıcılar oluşturulur:

### Admin Hesabı
- **Kullanıcı Adı:** `admin`
- **Şifre:** `admin123`

### Örnek Akademisyen Hesabı
- **Kullanıcı Adı:** `ahmet.yilmaz`
- **Şifre:** `123456`

### Örnek Öğrenci Hesabı
- **Kullanıcı Adı:** `mehmet.kaya`
- **Şifre:** `123456`

---

## Port Değiştirme (5178 kullanılıyorsa)

Eğer 5178 portu başka bir uygulama tarafından kullanılıyorsa:

1. `DersSunumSistemi/Properties/launchSettings.json` dosyasını açın
2. `applicationUrl` satırını bulun ve portu değiştirin:

```json
"applicationUrl": "http://localhost:5180"
```

3. Projeyi yeniden başlatın

---

## Sık Karşılaşılan Sorunlar ve Çözümler

### Sorun 1: "dotnet komut bulunamadı"
**Çözüm:**
- .NET 8.0 SDK'nın kurulu olduğundan emin olun
- Bilgisayarı yeniden başlatın
- Sistem PATH değişkenini kontrol edin

### Sorun 2: Veritabanı bağlantı hatası
**Çözüm:**
- SQL Server LocalDB'nin çalıştığından emin olun:
  ```bash
  sqllocaldb info
  sqllocaldb start mssqllocaldb
  ```
- `appsettings.json` dosyasındaki connection string'i kontrol edin

### Sorun 3: Port zaten kullanımda
**Çözüm:**
- Başka bir .NET uygulaması çalışıyor olabilir:
  ```bash
  # Windows'ta tüm dotnet süreçlerini kapat
  taskkill /F /IM dotnet.exe
  ```
- Veya yukarıdaki "Port Değiştirme" bölümünü takip edin

### Sorun 4: Migration hatası
**Çözüm:**
```bash
# Tüm migration'ları sil ve yeniden oluştur
dotnet ef database drop
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Sorun 5: wwwroot/images.png bulunamadı
**Çözüm:**
- Proje klasöründe `DersSunumSistemi/wwwroot` altına `images.png` dosyasının kopyalandığından emin olun
- Logo dosyası eksikse, geçici olarak herhangi bir .png dosyası kullanabilirsiniz

---

## Projeyi Durdurma

Terminal/Komut istemcisinde:
- `Ctrl + C` tuşlarına basın
- Veya terminali kapatın

---

## Geliştirme Modunda Çalıştırma (Hot Reload)

Kod değişikliklerinin otomatik yüklenmesi için:

```bash
dotnet watch run
```

Bu komutla, kod değiştirdiğinizde sayfa otomatik yenilenecek.

---

## Üretim Ortamında Yayınlama (Sunum için)

Eğer projeyi sunmak için yayınlamak isterseniz:

```bash
# Release modunda derle
dotnet publish -c Release -o ./publish

# Yayınlanan dosyalar ./publish klasöründe olacak
cd publish
dotnet DersSunumSistemi.dll
```

---

## Destek ve İletişim

Herhangi bir sorun yaşarsanız:
1. Hata mesajının tam metnini not alın
2. `dotnet --info` komutunun çıktısını alın
3. Proje sahibine ulaşın

---

## Ek Notlar

### wwwroot/uploads Klasörü
- Kullanıcıların yüklediği dosyalar `wwwroot/uploads` klasörüne kaydedilir
- Bu klasör otomatik oluşturulur
- Yedekleme yaparken bu klasörü de dahil edin

### Veritabanı Yedeği
Veritabanını yedeklemek için:
```bash
# LocalDB için
sqllocaldb stop mssqllocaldb
# .mdf ve .ldf dosyalarını kopyalayın
# Genelde: C:\Users\[Kullanici]\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\
```

### appsettings.Development.json
- Geliştirme ortamına özel ayarlar için kullanılır
- Production'da `appsettings.json` kullanılır

---

## Başarılı Kurulum Kontrolü

✅ Tamamlanması gereken kontrol listesi:

- [ ] .NET 8.0 SDK kuruldu (`dotnet --version` çalışıyor)
- [ ] SQL Server LocalDB kuruldu ve çalışıyor
- [ ] Proje klasörü kopyalandı
- [ ] `dotnet restore` başarılı
- [ ] `dotnet build` başarılı (0 hata)
- [ ] `dotnet run` ile proje başladı
- [ ] `http://localhost:5178` açılıyor
- [ ] Admin hesabıyla giriş yapılabiliyor (`admin` / `admin123`)

Tüm adımlar tamamlandıysa, proje başarıyla kurulmuştur! 🎉

---

**Son Güncelleme:** 14 Ekim 2025
**Proje Versiyonu:** .NET 8.0
**Veritabanı:** SQL Server / LocalDB
