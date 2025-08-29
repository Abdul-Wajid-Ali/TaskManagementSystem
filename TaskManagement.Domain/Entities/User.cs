using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }

        // Core fields
        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; } // Store hash, not raw password

        public string? PasswordSalt { get; set; }

        // Business rules
        public UserCreationMethod CreationMethod { get; set; }

        public UserRole Role { get; set; }

        // Tracking
        public long? CreatedByUserId { get; set; }
        public DateTime? CreatedOn { get; set; } 
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
