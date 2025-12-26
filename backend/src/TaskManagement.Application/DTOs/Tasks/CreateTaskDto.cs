namespace TaskManagement.Application.DTOs.Tasks;

public class CreateTaskDto
{
    public Guid? ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.TODO;
    public Domain.Enums.TaskPriority Priority { get; set; } = Domain.Enums.TaskPriority.MEDIUM;
    public DateTime? DueDate { get; set; }
}
