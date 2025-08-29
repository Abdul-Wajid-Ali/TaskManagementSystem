using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class Task
    {
        public long Id { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; } = default!;

        public TaskProgress Status { get; set; } = TaskProgress.Pending;

        // 🔹 Link to Admin (creator)
        public long CreatedByUserId { get; set; } = default!;

        public User CreatedByUser { get; set; } = null!;

        public DateOnly DueDate { get; set; } = default!;

        public DateTime? CompletedOn { get; set; }

        public DateTime CreatedOn { get; set; } = default!;

        public DateTime? UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        // 🔹 Many-to-Many with Employees
        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}