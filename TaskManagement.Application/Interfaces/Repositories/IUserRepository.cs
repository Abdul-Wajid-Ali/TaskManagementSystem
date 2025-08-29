using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateUserAsync(User user);

        Task<User?> GetUserByIdAsync(long id);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<bool> UpdateUserAsync(User user);

        Task<bool> SoftDeleteUserAsync(User user);
    }
}