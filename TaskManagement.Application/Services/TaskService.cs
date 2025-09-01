using AutoMapper;
using TaskManagement.Application.Common;
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

        // Create a new task
        public async Task<Result<long>> CreateTaskAsync(CreateTaskDto dto, long id)
        {
            var newTask = _mapper.Map<Task>(dto);

            newTask.CreatedByUserId = id;
            newTask.CreatedOn = DateTime.UtcNow;

            // If multiple users are assigned, handle the assignment
            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Count > 1)
                await _repository.AssignUsersToTaskAsync(newTask, dto.AssignedUserIds);

            await _repository.CreateTaskAsync(newTask);

            return Result<long>.Success(newTask.Id, SuccessCodes.TaskCreatedSuccessfully);
        }

        // Get a task by Id
        public async Task<Result<TaskDto>> GetTaskByIdAsync(long id)
        {
            var task = await _repository.GetTaskByIdAsync(id);

            var temp = _mapper.Map<TaskDto>(task);

            if (task == null)
                return Result<TaskDto>.Fail(ErrorCodes.TaskNotFound);

            return Result<TaskDto>.Success(temp!);
        }

        // Get all tasks assigned to a user
        public async Task<Result<IEnumerable<TaskDto>>> GetAssignedTasksAsync(long userId)
        {
            var userTasks = await _repository.GetAssignedTasksAsync(userId)
                .ContinueWith(item => _mapper.Map<List<TaskDto>>(item.Result));

            return Result<IEnumerable<TaskDto>>.Success(userTasks);
        }

        // Get all tasks created by specific user
        public async Task<Result<IEnumerable<TaskDto>>> GetCreatedTasksAsync(long userId)
        {
            var userTasks = await _repository.GetCreatedTasksAsync(userId)
                .ContinueWith(item => _mapper.Map<List<TaskDto>>(item.Result));

            return Result<IEnumerable<TaskDto>>.Success(userTasks);
        }

        // Update an existing task
        public async Task<Result<TaskDto>> UpdateTaskAsync(long taskId, UpdateTaskDto dto, long userId)
        {
            var existingTask = await _repository.GetTaskByIdAsync(taskId);

            // If task doesn't exist or deleted, return error
            if (existingTask == null)
                return Result<TaskDto>.Fail(ErrorCodes.TaskNotFound);

            var isTaskCreated = await _repository.IsCreatedTask(taskId, userId);

            // If task is not created by the user, return error
            if (!isTaskCreated)
                return Result<TaskDto>.Fail(ErrorCodes.TaskNotFound);

            // If multiple users are assigned, handle the assignment
            if (dto.AssignedUserIds != null && dto.AssignedUserIds.Count > 1)
                await _repository.AssignUsersToTaskAsync(existingTask, dto.AssignedUserIds);

            // Update only provided fields
            existingTask.Title = dto.Title ?? existingTask.Title;
            existingTask.Description = dto.Description ?? existingTask.Description;
            existingTask.Status = dto.Status;
            existingTask.DueDate = dto.DueDate ?? existingTask.DueDate;
            existingTask.UpdatedOn = DateTime.UtcNow;

            await _repository.UpdateTaskAsync(existingTask);

            return Result<TaskDto>.Success(_mapper.Map<TaskDto>(existingTask));
        }

        // Update only the status of a task
        public async Task<Result<TaskDto>> UpdateTaskStatusAsync(long taskId, UpdateTaskStatusDto dto, long userId)
        {
            var existingTask = await _repository.GetTaskByIdAsync(taskId);

            // If task doesn't exist or deleted, return error
            if (existingTask == null)
                return Result<TaskDto>.Fail(ErrorCodes.TaskNotFound);

            var isTaskAssigned = await _repository.IsAssignedTask(taskId, userId);

            // If task is not assigned to the user, return error
            if (!isTaskAssigned)
                return Result<TaskDto>.Fail(ErrorCodes.TaskNotFound);

            if (Enum.IsDefined(typeof(TaskStatus), dto.Status))
                return Result<TaskDto>.Fail(ErrorCodes.InvalidTaskStatus);

            // Update only status field
            existingTask.Status = dto.Status;
            existingTask.UpdatedOn = DateTime.UtcNow;

            await _repository.UpdateTaskAsync(existingTask);

            return Result<TaskDto>.Success(_mapper.Map<TaskDto>(existingTask));
        }

        // Soft delete a task
        public async Task<Result<bool>> SoftDeleteTaskAsync(long id, long userId)
        {
            var existingTask = await _repository.GetTaskByIdAsync(id);

            // If task doesn't exist, return false
            if (existingTask == null)
                return Result<bool>.Fail(ErrorCodes.TaskNotFound);

            var isTaskCreated = await _repository.IsCreatedTask(id, userId);

            // If task is not created by the user, return false
            if (!isTaskCreated)
                return Result<bool>.Fail(ErrorCodes.TaskNotFound);

            existingTask.DeletedOn = DateTime.UtcNow;

            return Result<bool>.Success(await _repository.UpdateTaskAsync(existingTask), SuccessCodes.TaskDeletedSucessfully);
        }
    }
}