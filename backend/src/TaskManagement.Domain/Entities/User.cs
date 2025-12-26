using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

    public string FullName => $"{FirstName} {LastName}";
}
