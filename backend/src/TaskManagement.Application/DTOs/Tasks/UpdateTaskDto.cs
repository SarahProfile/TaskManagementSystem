namespace TaskManagement.Application.DTOs.Tasks;

public class UpdateTaskDto
{
    public Guid? ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Domain.Enums.TaskStatus? Status { get; set; }
    public Domain.Enums.TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
