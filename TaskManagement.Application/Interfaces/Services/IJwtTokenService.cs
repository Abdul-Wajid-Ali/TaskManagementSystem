namespace TaskManagement.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(long userId, string email, IEnumerable<string> roles);
    }
}