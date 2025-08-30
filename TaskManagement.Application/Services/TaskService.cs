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

        public async Task<IEnumerable<TaskDto>?> GetAllTasksAsync()
        {
            var taskList = await _repository.GetAllTasksAsync()
                .ContinueWith(item => _mapper.Map<IEnumerable<TaskDto>>(item.Result));

            if (taskList == null || !taskList.Any())
                return null;

            return taskList;
        }

        public async Task<TaskDto?> GetTaskByIdAsync(long id)
        {
            var task = await _repository.GetTaskByIdAsync(id)
                .ContinueWith(item => _mapper.Map<TaskDto?>(item.Result));

            if (task == null)
                return null;

            return task;
        }

        public async Task<IEnumerable<TaskDto>?> GetUserTasks(long userId)
        {
            var userTasks = await _repository.GetUserTasks(userId)
                .ContinueWith(item => _mapper.Map<List<TaskDto>>(item.Result));

            if (userTasks == null || !userTasks.Any())
                return null;

            return userTasks;
        }

        public async Task<bool> SoftDeleteTaskAsync(long id)
        {
            var existingTask = await _repository.GetTaskByIdAsync(id);

            if (existingTask == null)
                return false;

            existingTask.DeletedOn = DateTime.UtcNow;

            return await _repository.UpdateTaskAsync(existingTask);
        }

        public async Task<bool> UpdateTaskAsync(long taskId, UpdateTaskDto dto)
        {
            var existingTask = await _repository.GetTaskByIdAsync(taskId);

            if (existingTask == null)
                return false;

            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Count > 1)
                await _repository.AssignUsersToTaskAsync(existingTask, dto.AssignedUserIds);

            existingTask.Title = dto.Title ?? existingTask.Title;
            existingTask.Description = dto.Description ?? existingTask.Description;
            existingTask.Status = dto.Status;
            existingTask.DueDate = dto.DueDate ?? existingTask.DueDate;
            existingTask.UpdatedOn = DateTime.UtcNow;

            return await _repository.UpdateTaskAsync(existingTask);
        }
    }
}