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
    pattern: "{controller=Home}/{action=Index}/{id?}");

// İlk admin kullanıcısı ve örnek verileri oluştur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        // Admin kullanıcı
        var admin = new User
        {
            UserName = "admin",
            Email = "admin@university.edu",
            PasswordHash = authService.HashPassword("admin123"),
            FullName = "Sistem Yöneticisi",
            Role = UserRole.Admin,
            IsActive = true,
            CreatedDate = DateTime.Now
        };
        context.Users.Add(admin);

        // Örnek bölümler
        var cseDepartment = new Department
        {
            Name = "Bilgisayar Mühendisliği",
            Code = "CSE",
            Description = "Bilgisayar Mühendisliği Bölümü"
        };

        var mathDepartment = new Department
        {
            Name = "Matematik",
            Code = "MATH",
            Description = "Matematik Bölümü"
        };

        context.Departments.AddRange(cseDepartment, mathDepartment);

        // Örnek akademisyen kullanıcı
        var instructorUser = new User
        {
            UserName = "ahmet.yilmaz",
            Email = "ahmet.yilmaz@university.edu",
            PasswordHash = authService.HashPassword("instructor123"),
            FullName = "Prof. Dr. Ahmet Yılmaz",
            Role = UserRole.Instructor,
            IsActive = true,
            CreatedDate = DateTime.Now
        };
        context.Users.Add(instructorUser);
        context.SaveChanges();

        var instructor = new Instructor
        {
            FullName = "Prof. Dr. Ahmet Yılmaz",
            Email = "ahmet.yilmaz@university.edu",
            Phone = "0532 123 4567",
            Title = "Prof. Dr.",
            Bio = "Bilgisayar Mühendisliği alanında 20 yıllık deneyim.",
            UserId = instructorUser.Id,
            DepartmentId = cseDepartment.Id,
            CreatedDate = DateTime.Now
        };
        context.Instructors.Add(instructor);

        // Örnek kategoriler
        var programmingCategory = new Category
        {
            Name = "Programlama",
            Description = "Programlama dilleri ve yazılım geliştirme"
        };

        var mathCategory = new Category
        {
            Name = "Matematik",
            Description = "Matematik dersleri"
        };

        context.Categories.AddRange(programmingCategory, mathCategory);
        context.SaveChanges();
    }
}

app.Run();