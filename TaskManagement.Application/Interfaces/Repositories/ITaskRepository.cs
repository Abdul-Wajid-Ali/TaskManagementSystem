using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<long> CreateTaskAsync(Task task);

        Task<Task?> GetTaskByIdAsync(long id);

        Task<IEnumerable<Task>> GetAssignedTasksAsync(long userId);

        Task<IEnumerable<Task>> GetCreatedTasksAsync(long userId);

        Task<bool> UpdateTaskAsync(Task task);

        Task<bool> AssignUsersToTaskAsync(Task task, List<long> userIds);

        Task<bool> IsCreatedTask(long taskId, long userId);

        Task<bool> IsAssignedTask(long taskId, long userId);

        Task<int> MarkCompletedTasksAsDeletedAsync(DateTime cutoff);
    }
}