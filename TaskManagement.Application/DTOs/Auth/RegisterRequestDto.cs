using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [MaxLength(300)]
        public string Email { get; set; } = default!;

        [MaxLength(200)]
        public string Username { get; set; } = default!;

        [MaxLength(1000)]
        public string Password { get; set; } = default!;

        public UserRole Role { get; set; } = default!;

        public long CreatedByUserId { get; set; } = default!;
    }
}
