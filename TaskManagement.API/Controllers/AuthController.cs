using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;
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
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 400)]
        public async Task<Result<UserDto>> Register([FromBody] RegisterRequestDto dto)
        {
            return await _authService.RegisterUserAsync(dto);
        }

        /// <summary>
        /// Authenticates a user and returns tokens.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>JWT and refresh token.</returns>
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 400)]
        [HttpPost("login")]
        public async Task<Result<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            return await _authService.LoginUserAsync(dto);
        }

        /// <summary>
        /// Generates new tokens using a refresh token.
        /// </summary>
        /// <param name="dto">Refresh token request.</param>
        /// <returns>New JWT and refresh token.</returns>
        [Authorize]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 400)]
        public async Task<Result<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            return await _authService.RefreshTokenAsync(dto);
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="dto">Change password request.</param>
        /// <returns>Success or failure response.</returns>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 400)]
        public async Task<Result<bool>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.GetCurrentUserId();

            return await _authService.ChangePasswordAsync(dto, (long)userId!);
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="dto">Get Logged in user profile</param>
        /// <returns>Success or failure response.</returns>
        [Authorize]
        [HttpPost("profile")]
        [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 400)]
        public async Task<Result<UserProfileDto>> GetProfile()
        {
            var userId = User.GetCurrentUserId();

            return await _authService.GetUserProfileAsync((long)userId!);
        }
    }
}