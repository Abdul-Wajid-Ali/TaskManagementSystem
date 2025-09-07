namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskCleanupService
    {
        Task<int> MarkCompletedTasksAsDeletedAsync();
    }
}