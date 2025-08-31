using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{    /// <summary>
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
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<Object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(new { data = new { id = result.Data } }, result.SuccessCode));
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
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<Object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<Object>.SuccessResponse(new { data = result.Data }));
        }

        /// <summary>
        /// Retrieves all Users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();

            return Ok(ApiResponse<Object>.SuccessResponse(new { data = result.Data }));
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="dto">User data for update.</param>
        /// <returns>Status of the update.</returns>
        [HttpPut("{id:long}/update")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCodes.UserNotFound
                        => NotFound(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    ErrorCodes.UserEmailAlreadyExists
                        => Conflict(ApiResponse<object>.FailResponse(result.ErrorCode)),

                    _ => BadRequest(ApiResponse<object>.FailResponse(ErrorCodes.InternalServerError))
                };

            return Ok(ApiResponse<object>.SuccessResponse(new { data = result.Data }, SuccessCodes.UserUpdatedSuccessfully));
        }

        /// <summary>
        /// Soft deletes a user by updating deletedOn DateTime field.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>Status of deletion.</returns>
        [HttpDelete("{id:long}/delete")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCodes.UserNotFound
                        => NotFound(ApiResponse<object>.FailResponse(result.ErrorCode)),
                    _ => BadRequest(ApiResponse<object>.FailResponse(ErrorCodes.InternalServerError))
                };

            return Ok(ApiResponse<object>.SuccessResponse(new { data = new { id } }, result.SuccessCode));
        }
    }
}