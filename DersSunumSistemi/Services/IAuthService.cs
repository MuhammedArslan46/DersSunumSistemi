using DersSunumSistemi.Models;

namespace DersSunumSistemi.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        Task<User?> AuthenticateAsync(string userName, string password);
        Task<User?> RegisterAsync(string userName, string email, string password, string fullName, UserRole role = UserRole.Student, int? departmentId = null);
    }
}
