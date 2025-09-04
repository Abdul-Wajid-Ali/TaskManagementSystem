using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Responses;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{    /// <summary>
     /// Controller for managing Authentication Operations.
     /// </summary>
     ///
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<Object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<Object>.SuccessResponse(result.Data!));
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<Object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<object>.FailResponse(result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Data!));
        }
    }
}