using AutoMapper;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Task mappings
        CreateMap<Domain.Entities.Task, TaskDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : null))
            .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.FullName : null));

        CreateMap<CreateTaskDto, Domain.Entities.Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedTo, opt => opt.Ignore());

        CreateMap<UpdateTaskDto, Domain.Entities.Task>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Project mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count));

        CreateMap<CreateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectUsers, opt => opt.Ignore());

        CreateMap<UpdateProjectDto, Project>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedProjects, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedTasks, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectUsers, opt => opt.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Auth mappings
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Domain.Enums.UserRole.User))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedProjects, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedTasks, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectUsers, opt => opt.Ignore());
    }
}
