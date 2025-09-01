using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<Result<long>> CreateTaskAsync(CreateTaskDto dto, long id);

        Task<Result<TaskDto>> GetTaskByIdAsync(long id);

        Task<Result<IEnumerable<TaskDto>>> GetAssignedTasksAsync(long userId);

        Task<Result<IEnumerable<TaskDto>>> GetCreatedTasksAsync(long userId);

        Task<Result<TaskDto>> UpdateTaskAsync(long taskId, UpdateTaskDto dto, long userId);

        Task<Result<TaskDto>> UpdateTaskStatusAsync(long taskId, UpdateTaskStatusDto dto, long userId);

        Task<Result<bool>> SoftDeleteTaskAsync(long id, long userId);
    }
}