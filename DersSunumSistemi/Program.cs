using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısını ekliyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session için gerekli (giriş sistemi için)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MVC servisini ekliyoruz
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata sayfası ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session'ı aktif ediyoruz

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// İlk admin kullanıcısını oluştur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    
    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            UserName = "admin",
            Password = "admin123",
            FullName = "Sistem Yöneticisi",
            IsAdmin = true,
            CreatedDate = DateTime.Now
        });
        context.SaveChanges();
    }
}

app.Run();