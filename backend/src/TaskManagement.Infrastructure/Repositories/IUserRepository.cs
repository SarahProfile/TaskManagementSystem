using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
