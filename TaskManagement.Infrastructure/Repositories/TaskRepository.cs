using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository(AppDbContext _dbContext) : ITaskRepository
    {
        // Create a new Task and return its Id
        public async Task<long> CreateTaskAsync(Task task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return task.Id;
        }

        // Get a task by Id only which are not deleted
        public async Task<Task?> GetTaskByIdAsync(long id)
        {
            return await _dbContext.Tasks.AsNoTracking()
                .Include(t => t.UserTasks)
                .Where(item => item.DeletedOn == null)
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        // Get tasks assigned to a specific user
        public async Task<IEnumerable<UserTask>> GetAssignedTasksAsync(long userId)
        {
            return await _dbContext.UsersTasks.AsNoTracking()
                .Where(item => item.UserId == userId)
                .Include(item => item.Task)
                .Where(item => item.Task.DeletedOn == null).ToListAsync();
        }

        // Get tasks created by a specific user
        public async Task<IEnumerable<Task>> GetCreatedTasksAsync(long userId)
        {
            return await _dbContext.Tasks.AsNoTracking()
                .Where(item => item.DeletedOn == null && item.CreatedByUserId == userId)
                .ToListAsync();
        }

        // Update an existing task
        public async Task<bool> UpdateTaskAsync(Task task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Assign multiple users to a task
        public async Task<bool> AssignUsersToTaskAsync(Task task, List<long> userIds)
        {
            foreach (var userId in userIds)
            {
                task.UserTasks.Add(new UserTask
                {
                    TaskId = task.Id,
                    UserId = userId
                });
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}