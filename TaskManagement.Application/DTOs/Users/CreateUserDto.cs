#pragma warning disable VSSpell001

using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Users
{
    public class CreateUserDto
    {
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!; 
        public UserRole Role { get; set; }
    }
}