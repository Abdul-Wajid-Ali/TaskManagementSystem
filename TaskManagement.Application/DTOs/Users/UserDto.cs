#pragma warning disable VSSpell001

using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Users
{
    public record UserDto
    {
        public long Id { get; init; }
        public string Email { get; init; } = default!;
        public string Username { get; init; } = default!;
        public UserCreationMethod CreationMethod { get; init; }
        public UserRole Role { get; init; }
        public long? CreatedByUserId { get; init; }
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; init; }
    }
}