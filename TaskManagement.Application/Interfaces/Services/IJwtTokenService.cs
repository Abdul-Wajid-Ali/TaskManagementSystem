using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserClaimsDto dto);

        string GenerateRefreshToken();
    }
}