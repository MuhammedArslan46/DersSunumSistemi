using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;
using DersSunumSistemi.Services;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısını ekliyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication servisleri
builder.Services.AddScoped<IAuthService, AuthService>();

// Cookie-based Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("InstructorOnly", policy => policy.RequireClaim("Role", "Instructor"));
    options.AddPolicy("InstructorOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Role" && (c.Value == "Instructor" || c.Value == "Admin"))));
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// İlk admin kullanıcısı ve örnek verileri oluştur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    // Seed örnek verileri
    await DataSeeder.SeedAsync(context);
}

app.Run();