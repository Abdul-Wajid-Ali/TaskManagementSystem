using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// Controller for managing tasks.
    /// </summary>
    [ApiController]
    [Route("api/task")]
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">Task data for creation.</param>
        /// <returns>Id of the newly created task with success message.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<long>), 200)]
        [ProducesResponseType(typeof(ApiResponse<long>), 400)]
        public async Task<Result<long>> CreateTask([FromBody] CreateTaskDto dto)
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            return await _taskService.CreateTaskAsync(dto, (long)userId!);
        }

        /// <summary>
        /// Retrieves a task by its Id.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Task details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 404)]
        public async Task<Result<TaskDto>> GetTask(long id)
        {
            return await _taskService.GetTaskByIdAsync(id);
        }

        /// <summary>
        /// Retrieves all tasks for createdby logged in user.
        /// </summary>
        /// <returns>Tasks List.</returns>
        [HttpGet("created")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 404)]
        public async Task<Result<IEnumerable<TaskDto>>> GetAllCreatedTasks()
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            return await _taskService.GetCreatedTasksAsync((long)userId!);
        }

        /// <summary>
        /// Retrieves all tasks assigned to logged-in user.
        /// </summary>
        /// <returns>Tasks List.</returns>
        [HttpGet("assigned")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 404)]
        public async Task<Result<IEnumerable<TaskDto>>> GetAllAssignedTasks()
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            return await _taskService.GetAssignedTasksAsync((long)userId!);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <param name="dto">Task data for update.</param>
        /// <returns>Status of the update.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:long}/update")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 400)]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 404)]
        public async Task<Result<TaskDto>> UpdateTask(long id, [FromBody] UpdateTaskDto dto)
        {
            var userId = User.GetCurrentUserId();

            return await _taskService.UpdateTaskAsync(id, dto, (long)userId!);
        }

        /// <summary>
        /// Updates an existing task's status.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <param name="dto">Task data for update.</param>
        /// <returns>Status of the update.</returns>
        [Authorize(Roles = "Employee")]
        [HttpPut("{id:long}/update/status")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 400)]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 404)]
        public async Task<Result<TaskDto>> UpdateTaskStatus(long id, [FromBody] UpdateTaskStatusDto dto)
        {
            var userId = User.GetCurrentUserId();

            return await _taskService.UpdateTaskStatusAsync(id, dto, (long)userId!);
        }

        /// <summary>
        /// Soft deletes a task by updating deletedOn DateTime field.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Status of deletion.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:long}/delete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
        public async Task<Result<bool>> DeleteTask(long id)
        {
            return await _taskService.SoftDeleteTaskAsync(id);
        }
    }
}