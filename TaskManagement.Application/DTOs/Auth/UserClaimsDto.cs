using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Auth
{
    public class UserClaimsDto
    {
        public long Id { get; init; } = default!;

        public string Email { get; init; } = default!;

        public string Username { get; init; } = default!;

        public UserRole Role { get; init; } = default!;
    }
}