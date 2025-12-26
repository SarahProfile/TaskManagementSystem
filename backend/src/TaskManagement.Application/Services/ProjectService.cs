using AutoMapper;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ProjectService(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ProjectDto>> GetAllProjectsAsync(int pageNumber = 1, int pageSize = 10)
    {
        var projects = await _projectRepository.GetAllAsync();
        var totalCount = projects.Count();

        var paginatedProjects = projects
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var projectDtos = _mapper.Map<List<ProjectDto>>(paginatedProjects);

        return new PaginatedResult<ProjectDto>
        {
            Items = projectDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ProjectDto> GetProjectByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, Guid createdById)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(createdById);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {createdById} not found. Please log in again.");
        }

        // Map DTO to entity
        var project = _mapper.Map<Project>(createProjectDto);
        project.CreatedById = createdById;
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = DateTime.UtcNow;

        // Save project
        await _projectRepository.AddAsync(project);

        // Reload project with navigation properties
        var createdProject = await _projectRepository.GetByIdAsync(project.Id);
        return _mapper.Map<ProjectDto>(createdProject);
    }

    public async Task<ProjectDto> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        // Map updates to entity
        _mapper.Map(updateProjectDto, project);
        project.UpdatedAt = DateTime.UtcNow;

        // Update project
        await _projectRepository.UpdateAsync(project);

        // Reload project with navigation properties
        var updatedProject = await _projectRepository.GetByIdAsync(id);
        return _mapper.Map<ProjectDto>(updatedProject);
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        await _projectRepository.DeleteAsync(project);
    }

    public async Task<List<ProjectDto>> GetProjectsByUserIdAsync(Guid userId)
    {
        var projects = await _projectRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}
