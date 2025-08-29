using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Extensions;
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
    public class AuthController(IUserService userService, IJwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(ApiResponse<Object>.FailResponse("Invalid User Data!"));

            dto.CreatedByUserId = User.GetCurrentUserId() ?? 0;

            var userId = await _userService.RegisterUserAsync(dto);
            return Ok(ApiResponse<long>.SuccessResponse(userId, "User Created Successfully."));
        }
    }
}