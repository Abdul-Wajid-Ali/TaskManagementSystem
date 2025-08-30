using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }

        // Core fields
        public string Email { get; set; } = default!; // Unique constraint in DB

        public string Username { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;// Store hash, not raw password

        public string PasswordSalt { get; set; } = default!;

        // Business rules
        public UserCreationMethod CreationMethod { get; set; } = default!;

        public UserRole Role { get; set; } = default!;

        // Tracking
        public long? CreatedByUserId { get; set; }

        public DateTime CreatedOn { get; set; } = default!;
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        // Navigation
        public User? CreatedByUser { get; set; }

        // 🔹 One Admin → Many Tasks
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();

        // 🔹 Many-to-Many with Tasks (for employees)
        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }
}