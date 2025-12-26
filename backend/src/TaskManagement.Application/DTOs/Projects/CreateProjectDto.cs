using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Projects;

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.ACTIVE;
    public Priority Priority { get; set; } = Priority.MEDIUM;
}
