using TaskManagement.Domain.Entities;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<long> CreateTaskAsync(Task task);

        Task<Task?> GetTaskByIdAsync(long id);

        Task<IEnumerable<UserTask>?> GetUserTasks(long userId);

        Task<IEnumerable<Task>?> GetAllTasksAsync();

        Task<bool> UpdateTaskAsync(Task task);

        Task<bool> AssignUsersToTaskAsync(Task task, List<long> userIds);
    }
}