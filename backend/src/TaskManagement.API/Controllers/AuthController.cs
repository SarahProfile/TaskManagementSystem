using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        _logger.LogInformation("User registration attempt for email: {Email}", request.Email);

        var result = await _authService.RegisterAsync(request);

        _logger.LogInformation("User successfully registered with ID: {UserId}", result.UserId);

        return CreatedAtAction(
            nameof(Register),
            new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "User registered successfully",
                Data = result
            });
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var result = await _authService.LoginAsync(request);

        _logger.LogInformation("User successfully logged in with ID: {UserId}", result.UserId);

        return Ok(new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Login successful",
            Data = result
        });
    }
}
