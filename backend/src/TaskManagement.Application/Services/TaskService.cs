using AutoMapper;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<TaskDto>> GetAllTasksAsync(int pageNumber = 1, int pageSize = 10)
    {
        var tasks = await _taskRepository.GetAllAsync();
        var totalCount = tasks.Count();

        var paginatedTasks = tasks
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var taskDtos = _mapper.Map<List<TaskDto>>(paginatedTasks);

        return new PaginatedResult<TaskDto>
        {
            Items = taskDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new NotFoundException($"Task with ID {id} not found");
        }

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        // Validate project exists if ProjectId is provided
        if (createTaskDto.ProjectId.HasValue)
        {
            var project = await _projectRepository.GetByIdAsync(createTaskDto.ProjectId.Value);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {createTaskDto.ProjectId} not found");
            }
        }

        // Map DTO to entity
        var task = _mapper.Map<Domain.Entities.Task>(createTaskDto);
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        // Save task
        await _taskRepository.AddAsync(task);

        // Reload task with navigation properties
        var createdTask = await _taskRepository.GetByIdAsync(task.Id);
        return _mapper.Map<TaskDto>(createdTask);
    }

    public async Task<TaskDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new NotFoundException($"Task with ID {id} not found");
        }

        // If ProjectId is being updated, validate it exists
        if (updateTaskDto.ProjectId.HasValue && updateTaskDto.ProjectId != task.ProjectId)
        {
            var project = await _projectRepository.GetByIdAsync(updateTaskDto.ProjectId.Value);
            if (project == null)
            {
                throw new NotFoundException($"Project with ID {updateTaskDto.ProjectId.Value} not found");
            }
        }

        // Map updates to entity
        _mapper.Map(updateTaskDto, task);
        task.UpdatedAt = DateTime.UtcNow;

        // Update task
        await _taskRepository.UpdateAsync(task);

        // Reload task with navigation properties
        var updatedTask = await _taskRepository.GetByIdAsync(id);
        return _mapper.Map<TaskDto>(updatedTask);
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new NotFoundException($"Task with ID {id} not found");
        }

        await _taskRepository.DeleteAsync(task);
    }

    public async Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId)
    {
        var tasks = await _taskRepository.GetByProjectIdAsync(projectId);
        return _mapper.Map<List<TaskDto>>(tasks);
    }

    public async Task<List<TaskDto>> GetTasksByUserIdAsync(Guid userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<TaskDto>>(tasks);
    }
}
