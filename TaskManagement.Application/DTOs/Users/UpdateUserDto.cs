using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        [MaxLength(300)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Username { get; set; }

        [MaxLength(1000)]
        public string? Password { get; set; } = default!;
    }
}
