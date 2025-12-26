using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<PaginatedResult<TaskDto>> GetAllTasksAsync(int pageNumber = 1, int pageSize = 10);
    Task<TaskDto> GetTaskByIdAsync(Guid id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
    Task<TaskDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto);
    Task DeleteTaskAsync(Guid id);
    Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId);
    Task<List<TaskDto>> GetTasksByUserIdAsync(Guid userId);
}
