using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// Controller for managing authentication operations.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">Registration details.</param>
        /// <returns>User details and tokens.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Authenticates a user and returns tokens.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>JWT and refresh token.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Generates new tokens using a refresh token.
        /// </summary>
        /// <param name="dto">Refresh token request.</param>
        /// <returns>New JWT and refresh token.</returns>
        [Authorize]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="dto">Change password request.</param>
        /// <returns>Success or failure response.</returns>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.GetCurrentUserId();
            var result = await _authService.ChangePasswordAsync(dto, (long)userId!);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="dto">Get Logged in user profile</param>
        /// <returns>Success or failure response.</returns>
        [Authorize]
        [HttpPost("profile")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.GetCurrentUserId();
            var result = await _authService.GetUserProfileAsync((long)userId!);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }
    }
}