using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(long userId, string email, string userName, UserRole role);
    }
}