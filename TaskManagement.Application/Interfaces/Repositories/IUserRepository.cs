using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateUserAsync(User user);

        Task<User?> GetUserByIdAsync(long id);

        Task<User?> GetUserByEmailAsync(string email);

        Task<IEnumerable<User>> GetCreatedUsersAsync(long id);

        Task<bool> UpdateUserAsync(User user);
    }
}