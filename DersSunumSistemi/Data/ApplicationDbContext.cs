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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
    }
}