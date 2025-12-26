namespace TaskManagement.Infrastructure.Repositories;

public interface ITaskRepository : IRepository<Domain.Entities.Task>
{
    Task<(IEnumerable<Domain.Entities.Task> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool isDescending = false);

    Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(Guid projectId);

    Task<IEnumerable<Domain.Entities.Task>> GetByUserIdAsync(Guid userId);
}
