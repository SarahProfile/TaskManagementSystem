using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data.Configurations;

public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.ToTable("ProjectUsers");

        // Composite primary key
        builder.HasKey(pu => new { pu.ProjectId, pu.UserId });

        builder.Property(pu => pu.ProjectId)
            .IsRequired();

        builder.Property(pu => pu.UserId)
            .IsRequired();

        builder.Property(pu => pu.AssignedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Indexes
        builder.HasIndex(pu => pu.ProjectId)
            .HasDatabaseName("IX_ProjectUsers_ProjectId");

        builder.HasIndex(pu => pu.UserId)
            .HasDatabaseName("IX_ProjectUsers_UserId");

        // Relationships
        builder.HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pu => pu.User)
            .WithMany(u => u.ProjectUsers)
            .HasForeignKey(pu => pu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
