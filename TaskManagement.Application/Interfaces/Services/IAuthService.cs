using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RegisterRequestDto> RegisterUserAsync(RegisterRequestDto dto);

        Task<LoginResponseDto?> LoginUserAsync(LoginRequestDto dto);

        Task<RefreshTokenRequestDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

        Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto dto);

        Task<UserProfileDto> GetUserProfileAsync();
    }
}
