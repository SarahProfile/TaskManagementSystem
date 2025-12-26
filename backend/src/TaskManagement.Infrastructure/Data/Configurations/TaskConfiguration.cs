using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.ProjectId)
            .IsRequired();

        builder.Property(t => t.AssignedToId);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.DueDate);

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Indexes
        builder.HasIndex(t => t.ProjectId)
            .HasDatabaseName("IX_Tasks_ProjectId");

        builder.HasIndex(t => t.AssignedToId)
            .HasDatabaseName("IX_Tasks_AssignedToId");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Tasks_Status");

        builder.HasIndex(t => t.Priority)
            .HasDatabaseName("IX_Tasks_Priority");

        builder.HasIndex(t => t.DueDate)
            .HasDatabaseName("IX_Tasks_DueDate");

        // Composite index for common queries
        builder.HasIndex(t => new { t.ProjectId, t.Status })
            .HasDatabaseName("IX_Tasks_ProjectId_Status");

        builder.HasIndex(t => new { t.AssignedToId, t.Status })
            .HasDatabaseName("IX_Tasks_AssignedToId_Status");

        // Relationships
        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.AssignedTo)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
