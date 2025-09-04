using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateUserAsync(User user);

        Task<User?> GetUserByIdAsync(long id);

        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

        Task<IEnumerable<User>> GetCreatedUsersAsync(long id);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> IsCreatedUser(long userId, long currentUserId);
    }
}