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
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid User Data!"));

            var userId = await _authService.RegisterUserAsync(dto);
            return Ok(ApiResponse<Object>.SuccessResponse(new { userId }, "User Created Successfully."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid User Data!"));

            var loggedInUser = await _authService.LoginUserAsync(dto);

            if (loggedInUser == null)
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid Credentials!"));

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(loggedInUser, "User Created Successfully."));
        }
    }
}