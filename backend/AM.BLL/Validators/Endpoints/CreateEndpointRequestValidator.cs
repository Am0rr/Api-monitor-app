using AM.BLL.DTOs.Endpoints;
using FluentValidation;

namespace AM.BLL.Validators.Endpoints;

public class CreateEndpointRequestValidator : AbstractValidator<CreateEndpointRequest>
{
    public CreateEndpointRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Endpoint name cannot be empty.")
            .MaximumLength(100).WithMessage("Endpoint name must not exceed 100 characters.");

        RuleFor(e => e.Url)
            .NotEmpty().WithMessage("Endpoint url cannot be empty.")
            .MaximumLength(500).WithMessage("Endpoint url must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("Endpoint url must be a valid HTTP or HTTPS address.");

        RuleFor(e => e.CheckIntervalSeconds)
            .GreaterThanOrEqualTo(5).WithMessage("Check interval must be at least 5 seconds.")
            .LessThanOrEqualTo(86400).WithMessage("Check interval cannot exceed 24 hours (86400 seconds).");
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}