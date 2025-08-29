#pragma warning disable VSSpell001

using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Users
{
    public record UserDto
    {
        public long Id { get; init; } = default!;

        public string Email { get; init; } = default!;

        public string Username { get; init; } = default!;

        public UserCreationMethod CreationMethod { get; init; } = default!;

        public UserRole Role { get; init; } = default!;

        public long? CreatedByUserId { get; init; }

        public DateTime CreatedOn { get; init; } = default!;

        public DateTime? UpdatedOn { get; init; }
    }
}