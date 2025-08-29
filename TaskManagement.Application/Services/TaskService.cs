using AutoMapper;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _repository;

        public TaskService(IMapper mapper, ITaskRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<long> CreateTaskAsync(CreateTaskDto dto)
        {
            var newTask = _mapper.Map<Task>(dto);

            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Count > 1)
                await _repository.AssignUsersToTaskAsync(newTask, dto.AssignedUserIds);

            await _repository.CreateTaskAsync(newTask);

            return newTask.Id;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            return await _repository.GetAllTasksAsync()
                .ContinueWith(item => _mapper.Map<IEnumerable<TaskDto>>(item.Result));
        }

        public async Task<TaskDto?> GetTaskByIdAsync(long id)
        {
            return await _repository.GetTaskByIdAsync(id)
                .ContinueWith(item => _mapper.Map<TaskDto?>(item.Result));
        }

        public async Task<bool> SoftDeleteTaskAsync(long id)
        {
            var existingTask = await _repository.GetTaskByIdAsync(id);

            if (existingTask == null)
                return false;

            existingTask.DeletedOn = DateTime.UtcNow;

            return await _repository.SoftDeleteTaskAsync(existingTask);
        }

        public async Task<bool> UpdateTaskAsync(long taskId, UpdateTaskDto dto)
        {
            var existingTask = await _repository.GetTaskByIdAsync(taskId);

            if (existingTask == null)
                return false;

            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Count > 1)
                await _repository.AssignUsersToTaskAsync(existingTask, dto.AssignedUserIds);

            existingTask.Title = dto.Title ?? existingTask.Title;
            existingTask.Description ??= dto.Description;
            existingTask.Status = dto.Status;
            existingTask.DueDate = dto.DueDate ?? existingTask.DueDate;
            existingTask.UpdatedOn = DateTime.UtcNow;

            return await _repository.UpdateTaskAsync(existingTask);
        }
    }
}