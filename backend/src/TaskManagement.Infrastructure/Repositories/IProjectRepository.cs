using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<(IEnumerable<Project> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool isDescending = false);

    Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);
}
