using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Project> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                (p.Description != null && p.Description.Contains(searchTerm)));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "name" => isDescending
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "status" => isDescending
                ? query.OrderByDescending(p => p.Status)
                : query.OrderBy(p => p.Status),
            "priority" => isDescending
                ? query.OrderByDescending(p => p.Priority)
                : query.OrderBy(p => p.Priority),
            "startdate" => isDescending
                ? query.OrderByDescending(p => p.StartDate)
                : query.OrderBy(p => p.StartDate),
            "createdat" => isDescending
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt),
            _ => isDescending
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt)
        };

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
            .Where(p => p.CreatedById == userId || p.ProjectUsers.Any(pu => pu.UserId == userId))
            .ToListAsync();
    }
}
