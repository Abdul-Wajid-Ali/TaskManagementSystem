using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{    /// <summary>
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            var result = await _taskService.CreateTaskAsync(dto, (long)userId!);

            return Ok(ApiResponse<object>.SuccessResponse(new { id = result.Data }, result.SuccessCode!));
        }

        /// <summary>
        /// Retrieves a task by its Id.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Task details.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetTask(long id)
        {
            var result = await _taskService.GetTaskByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<Object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<Object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Retrieves all tasks for createdby logged in user.
        /// </summary>
        /// <returns>Tasks List.</returns>
        [HttpGet("created")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetAllCreatedTasks()
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            var result = await _taskService.GetCreatedTasksAsync((long)userId!);

            return Ok(ApiResponse<Object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Retrieves all tasks assigned to logged-in user.
        /// </summary>
        /// <returns>Tasks List.</returns>
        [HttpGet("assigned")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetAllAssignedTasks()
        {
            // Get the current logged-in user's Id from the JWT token claims
            var userId = User.GetCurrentUserId();

            var result = await _taskService.GetAssignedTasksAsync((long)userId!);

            return Ok(ApiResponse<Object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <param name="dto">Task data for update.</param>
        /// <returns>Status of the update.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:long}/update")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateTask(long id, [FromBody] UpdateTaskDto dto)
        {
            var userId = User.GetCurrentUserId();

            var result = await _taskService.UpdateTaskAsync(id, dto, (long)userId!);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCodes.TaskNotFound
                        => NotFound(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    ErrorCodes.UserEmailAlreadyExists
                        => Conflict(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    _ => BadRequest(ApiResponse<object>.FailResponse(ErrorCodes.InternalServerError))
                };

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Updates an existing task's status.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <param name="dto">Task data for update.</param>
        /// <returns>Status of the update.</returns>
        [Authorize(Roles = "Employee")]
        [HttpPut("{id:long}/update/status")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateTaskStatus(long id, [FromBody] UpdateTaskStatusDto dto)
        {
            var userId = User.GetCurrentUserId();
            var result = await _taskService.UpdateTaskStatusAsync(id, dto, (long)userId!);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCodes.TaskNotFound
                        => NotFound(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    ErrorCodes.UserEmailAlreadyExists
                        => Conflict(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    _ => BadRequest(ApiResponse<object>.FailResponse(ErrorCodes.InternalServerError))
                };

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Soft deletes a task by updating deletedOn DateTime field.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Status of deletion.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:long}/delete")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var userId = User.GetCurrentUserId();
            var result = await _taskService.SoftDeleteTaskAsync(id, (long)userId!);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCodes.UserNotFound
                        => NotFound(ApiResponse<object>.FailResponse(result.ErrorCode)),
                    _ => BadRequest(ApiResponse<object>.FailResponse(ErrorCodes.InternalServerError))
                };

            return Ok(ApiResponse<object>.SuccessResponse(new { id }, result.SuccessCode!));
        }
    }
}