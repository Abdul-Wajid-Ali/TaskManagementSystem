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
        public async Task<IEnumerable<User>> GetCreatedUsersAsync(long id)
        {
            return await _dbContext.Users.AsNoTracking()
                .Where(item => item.DeletedOn == null && item.CreatedByUserId == id)
                .ToListAsync();
        }

        // Get a Users created by specifc user if not soft-deleted
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

        // Check if a user is created by the current user and not soft-deleted
        public Task<bool> IsCreatedUser(long userId, long currentUserId)
        {
            return _dbContext.Users.AsNoTracking()
                .AnyAsync(item => item.Id == userId && item.CreatedByUserId == currentUserId && item.DeletedOn == null);
        }

        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return _dbContext.Users.AsNoTracking()
                .Where(item => item.DeletedOn == null)
                .FirstOrDefaultAsync(item => string.Equals(item.RefreshToken, refreshToken));
        }
    }
}