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

        Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto dto, long id);

        Task<Result<UserProfileDto>> GetUserProfileAsync(long id);
    }
}