using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public record UpdateTaskStatusDto
    {
        public TaskProgress Status { get; init; } = default!;
    }
}