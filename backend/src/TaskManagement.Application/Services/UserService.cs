using AutoMapper;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<UserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
    {
        var users = await _userRepository.GetAllAsync();
        var totalCount = users.Count();

        var paginatedUsers = users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var userDtos = _mapper.Map<List<UserDto>>(paginatedUsers);

        return new PaginatedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (existingUser != null)
        {
            throw new BadRequestException("User with this email already exists");
        }

        // Map DTO to entity
        var user = _mapper.Map<User>(createUserDto);

        // Hash password using BCrypt
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        // Save user
        await _userRepository.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // If email is being updated, check for conflicts
        if (!string.IsNullOrEmpty(updateUserDto.Email) && updateUserDto.Email != user.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("User with this email already exists");
            }
        }

        // Map updates to entity
        _mapper.Map(updateUserDto, user);

        // If password is being updated, hash it
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
        }

        user.UpdatedAt = DateTime.UtcNow;

        // Update user
        await _userRepository.UpdateAsync(user);

        return _mapper.Map<UserDto>(user);
    }

    public async System.Threading.Tasks.Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        await _userRepository.DeleteAsync(user);
    }
}
