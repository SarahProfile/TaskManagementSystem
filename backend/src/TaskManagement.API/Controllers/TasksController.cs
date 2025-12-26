using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(
        ITaskService taskService,
        ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResult<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<TaskDto>>>> GetAllTasks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting all tasks - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var result = await _taskService.GetAllTasksAsync(pageNumber, pageSize);

        return Ok(new ApiResponse<PaginatedResult<TaskDto>>
        {
            Success = true,
            Message = "Tasks retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    /// Get task by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> GetTaskById(Guid id)
    {
        _logger.LogInformation("Getting task with ID: {TaskId}", id);

        var task = await _taskService.GetTaskByIdAsync(id);

        return Ok(new ApiResponse<TaskDto>
        {
            Success = true,
            Message = "Task retrieved successfully",
            Data = task
        });
    }

    /// <summary>
    /// Get tasks by project ID
    /// </summary>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(ApiResponse<List<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetTasksByProjectId(Guid projectId)
    {
        _logger.LogInformation("Getting tasks for project ID: {ProjectId}", projectId);

        var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);

        return Ok(new ApiResponse<List<TaskDto>>
        {
            Success = true,
            Message = "Tasks retrieved successfully",
            Data = tasks
        });
    }

    /// <summary>
    /// Get tasks by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<List<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetTasksByUserId(Guid userId)
    {
        _logger.LogInformation("Getting tasks for user ID: {UserId}", userId);

        var tasks = await _taskService.GetTasksByUserIdAsync(userId);

        return Ok(new ApiResponse<List<TaskDto>>
        {
            Success = true,
            Message = "Tasks retrieved successfully",
            Data = tasks
        });
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        _logger.LogInformation("Creating new task for project ID: {ProjectId}", createTaskDto.ProjectId);

        var task = await _taskService.CreateTaskAsync(createTaskDto);

        _logger.LogInformation("Task created successfully with ID: {TaskId}", task.Id);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = task.Id },
            new ApiResponse<TaskDto>
            {
                Success = true,
                Message = "Task created successfully",
                Data = task
            });
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        _logger.LogInformation("Updating task with ID: {TaskId}", id);

        var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);

        _logger.LogInformation("Task updated successfully with ID: {TaskId}", id);

        return Ok(new ApiResponse<TaskDto>
        {
            Success = true,
            Message = "Task updated successfully",
            Data = task
        });
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTask(Guid id)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", id);

        await _taskService.DeleteTaskAsync(id);

        _logger.LogInformation("Task deleted successfully with ID: {TaskId}", id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Task deleted successfully",
            Data = null
        });
    }
}
