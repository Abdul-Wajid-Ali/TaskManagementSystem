using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<long> CreateUserAsync(CreateUserDto dto);

        Task<long> RegisterUserAsync(RegisterRequestDto dto);

        Task<UserDto?> GetUserByIdAsync(long id);

        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<bool> UpdateUserAsync(long userId, UpdateUserDto dto);

        Task<bool> SoftDeleteUserAsync(long id);
    }
}