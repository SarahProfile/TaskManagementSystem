namespace TaskManagement.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.TODO;
    public Enums.TaskPriority Priority { get; set; } = Enums.TaskPriority.MEDIUM;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Project? Project { get; set; }
    public User? AssignedTo { get; set; }
}
