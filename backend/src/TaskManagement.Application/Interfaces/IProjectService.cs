using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;

namespace TaskManagement.Application.Interfaces;

public interface IProjectService
{
    Task<PaginatedResult<ProjectDto>> GetAllProjectsAsync(int pageNumber = 1, int pageSize = 10);
    Task<ProjectDto> GetProjectByIdAsync(Guid id);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, Guid createdById);
    Task<ProjectDto> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);
    Task DeleteProjectAsync(Guid id);
    Task<List<ProjectDto>> GetProjectsByUserIdAsync(Guid userId);
}
