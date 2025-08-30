using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{    /// <summary>
     /// Controller for managing tasks.
     /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">Task data for creation.</param>
        /// <returns>Id of the newly created task with success message.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid Task Data!"));

            var taskId = await _taskService.CreateTaskAsync(dto);
            return Ok(ApiResponse<long>.SuccessResponse(taskId, "Task Created Successfully."));
        }

        /// <summary>
        /// Retrieves a task by its Id.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Task details.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetTask(long id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
                return NotFound(ApiResponse<object>.FailResponse("Task not found"));

            return Ok(ApiResponse<TaskDto>.SuccessResponse(task));
        }

        /// <summary>
        /// Retrieves a task by its Id.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Task details.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = User.GetCurrentUserId();

            if (userId == null)
                return BadRequest(ApiResponse<object>.FailResponse("Invalid User Id"));

            var task = await _taskService.GetTaskByIdAsync((long)userId);

            if (task == null)
                return NotFound(ApiResponse<object>.FailResponse("Task not found"));

            return Ok(ApiResponse<TaskDto>.SuccessResponse(task));
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        /// <returns>List of tasks.</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 200)]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            if (tasks == null || !tasks.Any())
                return Ok(ApiResponse<IEnumerable<TaskDto>>.SuccessResponse([], "No tasks found"));

            return Ok(ApiResponse<IEnumerable<TaskDto>>.SuccessResponse(tasks));
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <param name="dto">Task data for update.</param>
        /// <returns>Status of the update.</returns>
        [HttpPut("{id:long}/update")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateTask(long id, [FromBody] UpdateTaskDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<object>.FailResponse("Invalid task data"));

            var updated = await _taskService.UpdateTaskAsync(id, dto);

            if (!updated)
                return NotFound(ApiResponse<object>.FailResponse("Task not found or already deleted"));

            return Ok(ApiResponse<object>.SuccessResponse(new { Id = id }, "Task updated successfully"));
        }

        /// <summary>
        /// Soft deletes a task by updating deletedOn DateTime field.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Status of deletion.</returns>
        [HttpDelete("{id:long}/delete")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var deleted = await _taskService.SoftDeleteTaskAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<object>.FailResponse("Task not found or already deleted"));

            return Ok(ApiResponse<object>.SuccessResponse(new { Id = id }, "Task deleted successfully"));
        }
    }
}