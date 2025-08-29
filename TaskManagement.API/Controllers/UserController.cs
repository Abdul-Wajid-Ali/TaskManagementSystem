using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Responses;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{    /// <summary>
     /// Controller for managing users.
     /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for UserController.
        /// </summary>
        /// <param name="userService">Service for user operations.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">User data for creation.</param>
        /// <returns>Id of the newly created user with success message.</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid User Data!"));

            var taskId = await _userService.CreateUserAsync(dto);
            return Ok(ApiResponse<long>.SuccessResponse(taskId, "User Created Successfully."));
        }

        /// <summary>
        /// Retrieves a user by its Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetUser(long id)
        {
            var task = await _userService.GetUserByIdAsync(id);

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
        /// Soft deletes a task by marking it as deleted.
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