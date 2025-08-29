#pragma warning disable VSSpell001

using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public record TaskDto
    {
        public long Id { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;
        public TaskProgress Status { get; init; }
        public long CreatedByUserId { get; set; }
        public List<long>? AssignedUserIds { get; init; } = new();
        public DateTime DueDate { get; init; } = default!;
        public DateTime CompletedOn { get; init; } = default!;
        public DateTime CreatedOn { get; init; } = default!;
        public DateTime UpdatedOn { get; init; } = default!;
    }
}