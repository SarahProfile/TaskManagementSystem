using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with pagination (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<UserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<UserDto>>>> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all users - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var result = await _userService.GetAllUsersAsync(pageNumber, pageSize);

        return Ok(new ApiResponse<PaginatedResult<UserDto>>
        {
            Success = true,
            Message = "Users retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(Guid id)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);

        var user = await _userService.GetUserByIdAsync(id);

        return Ok(new ApiResponse<UserDto>
        {
            Success = true,
            Message = "User retrieved successfully",
            Data = user
        });
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
    {
        _logger.LogInformation("Getting user with email: {Email}", email);

        var user = await _userService.GetUserByEmailAsync(email);

        return Ok(new ApiResponse<UserDto>
        {
            Success = true,
            Message = "User retrieved successfully",
            Data = user
        });
    }

    /// <summary>
    /// Create a new user (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        _logger.LogInformation("Creating new user with email: {Email}", createUserDto.Email);

        var user = await _userService.CreateUserAsync(createUserDto);

        _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = user.Id },
            new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User created successfully",
                Data = user
            });
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);

        var user = await _userService.UpdateUserAsync(id, updateUserDto);

        _logger.LogInformation("User updated successfully with ID: {UserId}", id);

        return Ok(new ApiResponse<UserDto>
        {
            Success = true,
            Message = "User updated successfully",
            Data = user
        });
    }

    /// <summary>
    /// Delete a user (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteUser(Guid id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);

        await _userService.DeleteUserAsync(id);

        _logger.LogInformation("User deleted successfully with ID: {UserId}", id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "User deleted successfully",
            Data = null
        });
    }
}
