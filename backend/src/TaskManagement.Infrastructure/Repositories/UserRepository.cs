using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public override async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.CreatedProjects)
            .Include(u => u.AssignedTasks)
            .Include(u => u.ProjectUsers)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.CreatedProjects)
            .Include(u => u.AssignedTasks)
            .Include(u => u.ProjectUsers)
            .ToListAsync();
    }
}
