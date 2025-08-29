using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Responses;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for UserController.
        /// </summary>
        /// <param name="userService">Service for user operations.</param>
        public UserController(IMapper mapper,IUserService userService)
        {
            _mapper = mapper;
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


            var userId = await _userService.CreateUserAsync(dto);
            return Ok(ApiResponse<long>.SuccessResponse(userId, "User Created Successfully."));
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
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse<object>.FailResponse("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user));
        }

        /// <summary>
        /// Retrieves all Users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(users));
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
            if (dto == null)
                return BadRequest(ApiResponse<object>.FailResponse("Invalid User data"));

            var userDto = _mapper.Map<UpdateUserDto>(dto);

            var updated = await _userService.UpdateUserAsync(id, dto);

            if (!updated)
                return NotFound(ApiResponse<object>.FailResponse("User not found or already deleted"));

            return Ok(ApiResponse<object>.SuccessResponse(new { Id = id }, "User updated successfully"));
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
            var deleted = await _userService.SoftDeleteUserAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<object>.FailResponse("User not found or already deleted"));

            return Ok(ApiResponse<object>.SuccessResponse(new { Id = id }, "User deleted successfully"));
        }
    }
}