using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.ACTIVE;
    public Priority Priority { get; set; } = Priority.MEDIUM;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User CreatedBy { get; set; } = null!;
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}
