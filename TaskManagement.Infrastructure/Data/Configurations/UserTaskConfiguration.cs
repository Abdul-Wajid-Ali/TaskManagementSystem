using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            // Table name
            builder.ToTable("UsersTasks");

            builder.HasKey(ut => new { ut.UserId, ut.TaskId });

            builder.HasOne(ut => ut.User)
                   .WithMany(u => u.UserTasks)
                   .HasForeignKey(ut => ut.UserId);

            builder.HasOne(ut => ut.Task)
                   .WithMany(t => t.UserTasks)
                   .HasForeignKey(ut => ut.TaskId);
        }
    }
}
