using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Enums;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            // Table name
            builder.ToTable("Tasks");

            // Primary Key
            builder.HasKey(t => t.Id);

            // Title - Required
            builder.Property(t => t.Title)
                   .IsRequired(true)
                   .HasMaxLength(200);

            // Description - Optional (nullable by default)
            builder.Property(t => t.Description).IsRequired(false).HasMaxLength(2000);

            // Status - Required, default value = Pending
            builder.Property(t => t.Status)
                   .IsRequired(true)
                   .HasDefaultValue(TaskProgress.Pending);

            // DueDate - Optional
            builder.Property(t => t.DueDate).IsRequired(false);

            // CompletedOn - Optional
            builder.Property(t => t.CompletedOn).IsRequired(false); 

            // CreatedOn - Only set on add, default value = current UTC date/time
            builder.Property(t => t.CreatedOn).ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");

            // UpdatedOn - Optional
            builder.Property(t => t.UpdatedOn).IsRequired(false);

            // DeletedOn - Optional
            builder.Property(t => t.DeletedOn).IsRequired(false);

            // 🔹 One Admin → Many Tasks
            builder.HasOne(t => t.CreatedByUser)
                   .WithMany(u => u.CreatedTasks)
                   .HasForeignKey(t => t.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}