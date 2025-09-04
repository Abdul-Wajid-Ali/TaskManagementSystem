using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<UserDto>> RegisterUserAsync(RegisterRequestDto dto);

        Task<Result<LoginResponseDto>> LoginUserAsync(LoginRequestDto dto);

        Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto);

        Task<Result<ChangePasswordDto>> ChangePasswordAsync(ChangePasswordDto dto);

        Task<Result<UserProfileDto>> GetUserProfileAsync();
    }
}