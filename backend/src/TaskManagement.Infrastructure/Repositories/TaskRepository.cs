using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Domain.Entities.Task?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public override async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Domain.Entities.Task> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t =>
                t.Title.Contains(searchTerm) ||
                (t.Description != null && t.Description.Contains(searchTerm)));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "title" => isDescending
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),
            "status" => isDescending
                ? query.OrderByDescending(t => t.Status)
                : query.OrderBy(t => t.Status),
            "priority" => isDescending
                ? query.OrderByDescending(t => t.Priority)
                : query.OrderBy(t => t.Priority),
            "duedate" => isDescending
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate),
            "createdat" => isDescending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),
            _ => isDescending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt)
        };

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .Where(t => t.AssignedToId == userId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }
}
