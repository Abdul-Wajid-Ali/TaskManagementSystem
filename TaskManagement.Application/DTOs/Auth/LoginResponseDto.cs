using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public long Id { get; set; }

        [MaxLength(300)]
        public string Email { get; set; } = default!;

        [MaxLength(200)]
        public string Username { get; set; } = default!;

        public UserCreationMethod CreationMethod { get; set; } = default;

        public UserRole Role { get; set; } = default!;

        public long? CreatedByUserId { get; init; }

        public DateTime CreatedOn { get; init; } = default!;

        public string Token { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
    }
}