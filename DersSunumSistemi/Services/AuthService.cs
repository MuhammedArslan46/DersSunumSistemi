using BCrypt.Net;
using DersSunumSistemi.Data;
using DersSunumSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace DersSunumSistemi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public async Task<User?> AuthenticateAsync(string userName, string password)
        {
            var user = await _context.Users
                .Include(u => u.Instructor)
                .FirstOrDefaultAsync(u => u.UserName == userName && u.IsActive);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> RegisterAsync(string userName, string email, string password, string fullName, UserRole role = UserRole.Student)
        {
            // Kullanıcı adı veya email zaten var mı kontrol et
            if (await _context.Users.AnyAsync(u => u.UserName == userName || u.Email == email))
            {
                return null;
            }

            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = HashPassword(password),
                FullName = fullName,
                Role = role,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
