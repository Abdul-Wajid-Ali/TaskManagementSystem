using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/user")]
    public class UserController(IUserService _userService) : ControllerBase
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">User data for creation.</param>
        /// <returns>Id of the newly created user with success message.</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<long>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<Result<long>> CreateUser([FromBody] CreateUserDto dto)
        {
            var currentUserId = User.GetCurrentUserId();

            return await _userService.CreateUserAsync(dto, (long)currentUserId!);
        }

        /// <summary>
        /// Retrieves a user by its Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<Result<UserDto>> GetUser(long id)
        {
            var currentUserId = User.GetCurrentUserId();

            return await _userService.GetUserByIdAsync(id, (long)currentUserId!);
        }

        /// <summary>
        /// Retrieves all Users created by loggedIn Admin.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet("created")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), 200)]
        public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
        {
            var userId = User.GetCurrentUserId();

            return await _userService.GetCreatedUsersAsync((long)userId!);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="dto">User data for update.</param>
        /// <returns>Status of the update.</returns>
        [HttpPut("{id:long}/update")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<Result<UserDto>> UpdateUser(long id, [FromBody] UpdateUserDto dto)
        {
            var currentUserId = User.GetCurrentUserId();

            return await _userService.UpdateUserAsync(id, dto, (long)currentUserId!);
        }

        /// <summary>
        /// Soft deletes a user by updating deletedOn DateTime field.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Status of deletion.</returns>
        [HttpDelete("{id:long}/delete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<Result<bool>> DeleteUser(long id)
        {
            var currentUserId = User.GetCurrentUserId();
            return await _userService.SoftDeleteUserAsync(id, (long)currentUserId!);
        }
    }
}