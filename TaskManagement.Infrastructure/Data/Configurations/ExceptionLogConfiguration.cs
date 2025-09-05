using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data.Configurations;

public class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
{
       public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ExceptionLog> builder)
       {
              builder.ToTable("ExceptionLogs");

              builder.HasKey(r => r.Id);

              builder.Property(r => r.UserId).IsRequired(false);

              builder.Property(r => r.Method)
                     .IsRequired(true)
                     .HasMaxLength(100);

              builder.Property(r => r.EndPoint)
                     .IsRequired(true)
                     .HasMaxLength(2048);

              builder.Property(r => r.ExceptionName)
                     .IsRequired(true)
                     .HasMaxLength(256);

              builder.Property(r => r.Message)
                     .IsRequired(true)
                     .HasColumnType("varchar(max)");

              builder.Property(r => r.StackTrace)
                     .IsRequired(true)
                     .HasColumnType("varchar(max)");

              builder.Property(r => r.LoggedAt)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("GETUTCDATE()");
       }
}
