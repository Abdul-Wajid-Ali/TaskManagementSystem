using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<long>> CreateUserAsync(CreateUserDto dto, long Id);

        Task<Result<UserDto>> GetUserByIdAsync(long id, long userId);

        Task<Result<IEnumerable<UserDto>>> GetCreatedUsersAsync(long id);

        Task<Result<UserDto>> UpdateUserAsync(long userId, UpdateUserDto dto, long currentUserId);

        Task<Result<bool>> SoftDeleteUserAsync(long id, long userId);
    }
}