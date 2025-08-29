using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<long> CreateUserAsync(CreateUserDto dto);

        Task<UserDto?> GetUserByIdAsync(long id);

        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<bool> UpdateUserAsync(long UserId, UpdateUserDto dto);

        Task<bool> SoftDeleteUserAsync(long id);
    }
}