using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static async System.Threading.Tasks.Task SeedDataAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Users.Any())
        {
            return; // Database has been seeded
        }

        // Create users with BCrypt hashed passwords
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@taskmanagement.com",
            FirstName = "Admin",
            LastName = "User",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var regularUser1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "john.doe@taskmanagement.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var regularUser2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane.smith@taskmanagement.com",
            FirstName = "Jane",
            LastName = "Smith",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddRangeAsync(adminUser, regularUser1, regularUser2);
        await context.SaveChangesAsync();

        // Create projects
        var project1 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Task Management System",
            Description = "A comprehensive task management system with project tracking capabilities",
            CreatedById = adminUser.Id,
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(6),
            Status = ProjectStatus.ACTIVE,
            Priority = Priority.HIGH,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var project2 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Mobile App Development",
            Description = "Development of mobile application for iOS and Android platforms",
            CreatedById = adminUser.Id,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(4),
            Status = ProjectStatus.ACTIVE,
            Priority = Priority.MEDIUM,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Projects.AddRangeAsync(project1, project2);
        await context.SaveChangesAsync();

        // Create project-user assignments
        var projectUser1 = new ProjectUser
        {
            ProjectId = project1.Id,
            UserId = regularUser1.Id,
            AssignedAt = DateTime.UtcNow
        };

        var projectUser2 = new ProjectUser
        {
            ProjectId = project1.Id,
            UserId = regularUser2.Id,
            AssignedAt = DateTime.UtcNow
        };

        var projectUser3 = new ProjectUser
        {
            ProjectId = project2.Id,
            UserId = regularUser1.Id,
            AssignedAt = DateTime.UtcNow
        };

        await context.ProjectUsers.AddRangeAsync(projectUser1, projectUser2, projectUser3);
        await context.SaveChangesAsync();

        // Create tasks
        var task1 = new Domain.Entities.Task
        {
            Id = Guid.NewGuid(),
            ProjectId = project1.Id,
            AssignedToId = regularUser1.Id,
            Title = "Design database schema",
            Description = "Create comprehensive database schema for the task management system",
            Status = Domain.Enums.TaskStatus.DONE,
            Priority = TaskPriority.HIGH,
            DueDate = DateTime.UtcNow.AddDays(-5),
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            UpdatedAt = DateTime.UtcNow.AddDays(-5)
        };

        var task2 = new Domain.Entities.Task
        {
            Id = Guid.NewGuid(),
            ProjectId = project1.Id,
            AssignedToId = regularUser2.Id,
            Title = "Implement authentication API",
            Description = "Develop JWT-based authentication with login and registration endpoints",
            Status = Domain.Enums.TaskStatus.IN_PROGRESS,
            Priority = TaskPriority.HIGH,
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            UpdatedAt = DateTime.UtcNow.AddDays(-2)
        };

        var task3 = new Domain.Entities.Task
        {
            Id = Guid.NewGuid(),
            ProjectId = project1.Id,
            AssignedToId = regularUser1.Id,
            Title = "Create project management endpoints",
            Description = "Implement CRUD operations for project management",
            Status = Domain.Enums.TaskStatus.TODO,
            Priority = TaskPriority.MEDIUM,
            DueDate = DateTime.UtcNow.AddDays(14),
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            UpdatedAt = DateTime.UtcNow.AddDays(-5)
        };

        var task4 = new Domain.Entities.Task
        {
            Id = Guid.NewGuid(),
            ProjectId = project2.Id,
            AssignedToId = regularUser1.Id,
            Title = "Setup mobile development environment",
            Description = "Configure React Native development environment with necessary dependencies",
            Status = Domain.Enums.TaskStatus.DONE,
            Priority = TaskPriority.HIGH,
            DueDate = DateTime.UtcNow.AddDays(-2),
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            UpdatedAt = DateTime.UtcNow.AddDays(-2)
        };

        var task5 = new Domain.Entities.Task
        {
            Id = Guid.NewGuid(),
            ProjectId = project2.Id,
            AssignedToId = null,
            Title = "Design mobile UI mockups",
            Description = "Create user interface mockups for mobile application screens",
            Status = Domain.Enums.TaskStatus.TODO,
            Priority = TaskPriority.LOW,
            DueDate = DateTime.UtcNow.AddDays(10),
            CreatedAt = DateTime.UtcNow.AddDays(-3),
            UpdatedAt = DateTime.UtcNow.AddDays(-3)
        };

        await context.Tasks.AddRangeAsync(task1, task2, task3, task4, task5);
        await context.SaveChangesAsync();
    }
}
