using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<long> CreateTaskAsync(CreateTaskDto dto);

        Task<TaskDto?> GetTaskByIdAsync(long id);

        Task<IEnumerable<TaskDto>?> GetUserTasks(long userId);

        Task<IEnumerable<TaskDto>?> GetAllTasksAsync();

        Task<bool> UpdateTaskAsync(long taskId, UpdateTaskDto dto);

        Task<bool> SoftDeleteTaskAsync(long id);
    }
}