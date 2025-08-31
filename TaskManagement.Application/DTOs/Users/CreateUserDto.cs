#pragma warning disable VSSpell001

using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Users
{
    public class CreateUserDto
    {
        [MaxLength(300)]
        public string Email { get; set; } = default!;

        [MaxLength(200)]
        public string Username { get; set; } = default!;

        [MaxLength(1000)]
        public string Password { get; set; } = default!;
    }
}