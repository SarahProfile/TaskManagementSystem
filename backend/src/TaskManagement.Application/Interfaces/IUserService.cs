using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces;

public interface IUserService
{
    Task<PaginatedResult<UserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task DeleteUserAsync(Guid id);
}
