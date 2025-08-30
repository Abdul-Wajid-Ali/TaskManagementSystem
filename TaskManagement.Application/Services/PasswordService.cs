using System.Security.Cryptography;
using System.Text;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class PasswordService : IPasswordService
    {
        // Generate salt
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        // Hash password with salt
        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hash);
        }

        // Verify password
        public bool VerifyPassword(string password, string salt, string hash)
        {
            var enteredHash = HashPassword(password, salt);
            return String.Equals(hash, enteredHash);
        }
    }
}