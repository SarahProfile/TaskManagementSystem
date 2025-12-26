using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Projects;

public class UpdateProjectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus? Status { get; set; }
    public Priority? Priority { get; set; }
}
