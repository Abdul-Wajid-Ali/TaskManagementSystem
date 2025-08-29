using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [MaxLength(300)]
        public string Email { get; set; } = default!;

        [MaxLength(1000)]
        public string Password { get; set; } = default!;
    }
}