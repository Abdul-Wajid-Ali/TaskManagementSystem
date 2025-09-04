using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(300)]
        public string Email { get; set; } = default!;

        [Required]
        [MaxLength(1000)]
        public string Password { get; set; } = default!;
    }
}