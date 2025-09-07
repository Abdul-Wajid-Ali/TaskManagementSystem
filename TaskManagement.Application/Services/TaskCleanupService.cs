using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class TaskCleanupService : ITaskCleanupService
    {
        private readonly ITaskRepository _repository;

        public TaskCleanupService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> MarkCompletedTasksAsDeletedAsync()
        {
            return await _repository.MarkCompletedTasksAsDeletedAsync(DateTime.UtcNow.AddDays(-5));
        }
    }
}