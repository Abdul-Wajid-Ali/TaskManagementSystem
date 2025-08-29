#pragma warning disable VSSpell001

using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public record CreateTaskDto
    {
        [Required, MaxLength(200)]
        public string? Title { get; init; }

        [MaxLength(2000)]
        public string? Description { get; init; }
        public long CreatedByUserId { get; set; }
        public List<long>? AssignedUserIds { get; init; } = new();

        [DataType(DataType.Date)]
        public DateOnly? DueDate { get; init; }
    }
}
