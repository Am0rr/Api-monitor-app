using AM.BLL.DTOs.ExecutionLogs;
using FluentValidation;

namespace AM.BLL.Validators.ExecutionLogs;

public class CreateExecutionLogRequestValidator : AbstractValidator<CreateExecutionLogRequest>
{
    public CreateExecutionLogRequestValidator()
    {
        RuleFor(l => l.EndpointId)
            .NotEmpty().WithMessage("EndpointId must be a valid non-empty GUID.");
        
        RuleFor(l => l.StatusCode)
            .InclusiveBetween(100, 599)
            .When(l => l.StatusCode.HasValue)
            .WithMessage("HTTP status code must be between 100 and 599.");

        RuleFor(l => l.ResponseTimeMs)
            .GreaterThanOrEqualTo(0).WithMessage("Response time cannot be negative.")
            .LessThanOrEqualTo(60000).WithMessage("Response time cannot exceed 60 000 ms (60 seconds).");
        
        RuleFor(l => l.ErrorMessage)
            .MaximumLength(4000)
            .When(l => l.ErrorMessage != null)
            .WithMessage("Error message must not exceed 4000 characters.");
    }
}