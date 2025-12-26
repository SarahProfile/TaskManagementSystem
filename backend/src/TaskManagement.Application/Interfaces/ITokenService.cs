using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
}
