using FluentValidation;
using TaskManagement.Application.DTOs.Projects;

namespace TaskManagement.Application.Validators.Projects;

public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1)).WithMessage("Start date cannot be more than 1 year in the future");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid project status");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority");
    }
}
