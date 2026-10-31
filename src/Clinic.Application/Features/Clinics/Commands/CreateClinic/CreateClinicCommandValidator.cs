namespace Clinic.Application.Features.Clinics.Commands.CreateClinic;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateClinicCommand"/>.
/// </summary>
public class CreateClinicCommandValidator : AbstractValidator<CreateClinicCommand>
{
    /// <summary>Initialises validation rules for creating a clinic.</summary>
    public CreateClinicCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.OpeningTime).NotEmpty().Must(t => TimeSpan.TryParse(t, out _))
            .WithMessage("OpeningTime must be a valid time in HH:mm format.");
        RuleFor(x => x.ClosingTime).NotEmpty().Must(t => TimeSpan.TryParse(t, out _))
            .WithMessage("ClosingTime must be a valid time in HH:mm format.");
        RuleFor(x => x.SlotDurationMinutes).GreaterThan(0).LessThanOrEqualTo(240);
    }
}
