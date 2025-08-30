using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext _dbContext) : IUserRepository
    {
        // Create a new User and return its Id
        public async Task<long> CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        // Get all Users that are not soft-deleted
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.AsNoTracking()
                .Where(t => t.DeletedOn == null)
                .ToListAsync();
        }

        // Get a User by Id if not soft-deleted
        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _dbContext.Users.AsNoTracking()
                .Where(item => item.DeletedOn == null)
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        // Get a User by Email if not soft-deleted
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking()
                .Where(item => item.DeletedOn == null)
                .FirstOrDefaultAsync(item => string.Equals(item.Email, email));
        }

        // Update/Soft Delete an existing User
        public async Task<bool> UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}