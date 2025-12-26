using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        IProjectService projectService,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Get all projects with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ProjectDto>>>> GetAllProjects(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all projects - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var result = await _projectService.GetAllProjectsAsync(pageNumber, pageSize);

        return Ok(new ApiResponse<PaginatedResult<ProjectDto>>
        {
            Success = true,
            Message = "Projects retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    /// Get project by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProjectById(Guid id)
    {
        _logger.LogInformation("Getting project with ID: {ProjectId}", id);

        var project = await _projectService.GetProjectByIdAsync(id);

        return Ok(new ApiResponse<ProjectDto>
        {
            Success = true,
            Message = "Project retrieved successfully",
            Data = project
        });
    }

    /// <summary>
    /// Get projects by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<List<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ProjectDto>>>> GetProjectsByUserId(Guid userId)
    {
        _logger.LogInformation("Getting projects for user ID: {UserId}", userId);

        var projects = await _projectService.GetProjectsByUserIdAsync(userId);

        return Ok(new ApiResponse<List<ProjectDto>>
        {
            Success = true,
            Message = "Projects retrieved successfully",
            Data = projects
        });
    }

    /// <summary>
    /// Create a new project
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto createProjectDto)
    {
        // Get the user ID from the JWT token claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid user token",
                Data = null
            });
        }

        _logger.LogInformation("Creating new project by user ID: {UserId}", userId);

        var project = await _projectService.CreateProjectAsync(createProjectDto, userId);

        _logger.LogInformation("Project created successfully with ID: {ProjectId}", project.Id);

        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = project.Id },
            new ApiResponse<ProjectDto>
            {
                Success = true,
                Message = "Project created successfully",
                Data = project
            });
    }

    /// <summary>
    /// Update an existing project
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(Guid id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        _logger.LogInformation("Updating project with ID: {ProjectId}", id);

        var project = await _projectService.UpdateProjectAsync(id, updateProjectDto);

        _logger.LogInformation("Project updated successfully with ID: {ProjectId}", id);

        return Ok(new ApiResponse<ProjectDto>
        {
            Success = true,
            Message = "Project updated successfully",
            Data = project
        });
    }

    /// <summary>
    /// Delete a project
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteProject(Guid id)
    {
        _logger.LogInformation("Deleting project with ID: {ProjectId}", id);

        await _projectService.DeleteProjectAsync(id);

        _logger.LogInformation("Project deleted successfully with ID: {ProjectId}", id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Project deleted successfully",
            Data = null
        });
    }
}
