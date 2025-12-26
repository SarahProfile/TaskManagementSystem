using FluentValidation;
using TaskManagement.Application.DTOs.Projects;

namespace TaskManagement.Application.Validators.Projects;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name cannot be empty")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1)).WithMessage("Start date cannot be more than 1 year in the future")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate!.Value).WithMessage("End date must be after start date")
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid project status")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority")
            .When(x => x.Priority.HasValue);
    }
}
