namespace TaskManagement.Domain.Entities;

public class ProjectUser
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
}
