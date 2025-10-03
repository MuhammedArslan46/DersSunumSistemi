using Microsoft.EntityFrameworkCore;
using DersSunumSistemi.Models;

namespace DersSunumSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Presentation> Presentations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Instructor ilişkisi (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Instructor)
                .WithOne(i => i.User)
                .HasForeignKey<Instructor>(i => i.UserId);

            // İndeksler
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.Code);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Code)
                .IsUnique();
        }
    }
}