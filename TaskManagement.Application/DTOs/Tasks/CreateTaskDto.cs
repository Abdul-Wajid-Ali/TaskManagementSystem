#pragma warning disable VSSpell001

using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public record CreateTaskDto
    {
        [MaxLength(200)]
        public string Title { get; init; } = default!;

        [MaxLength(2000)]
        public string Description { get; init; } = default!;

        public TaskProgress Status { get; init; } = default!;

        public List<long>? AssignedUserIds { get; init; } = new();

        public DateOnly DueDate { get; init; } = default!;
    }
}
