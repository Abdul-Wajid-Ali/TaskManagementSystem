using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired(true)
                .HasMaxLength(300);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.Username)
                .IsRequired(true)
                .HasMaxLength(300);

            builder.Property(u => u.PasswordHash)
                .IsRequired(true)
                .HasMaxLength(1000);

            builder.Property(u => u.PasswordSalt)
                .IsRequired(true)
                .HasMaxLength(1000);

            builder.HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // CreatedOn - Only set on add, default value = current UTC date/time
            builder.Property(t => t.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");

            // UpdatedOn - Optional
            builder.Property(t => t.UpdatedOn)
                .IsRequired(false);

            // DeletedOn - Optional
            builder.Property(t => t.DeletedOn)
                .IsRequired(false);

            builder.Property(u => u.CreationMethod)
                .IsRequired(true).HasDefaultValue(UserCreationMethod.CreatedByAdmin);

            builder.Property(u => u.Role)
                .IsRequired(true).HasDefaultValue(UserRole.Employee);
        }
    }
}