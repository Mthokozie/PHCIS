namespace Clinic.Application.Features.Patients.Commands.CreatePatient;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreatePatientCommand"/>.
/// </summary>
public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    /// <summary>Initialises validation rules for creating a patient.</summary>
    public CreatePatientCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.IdNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");
    }
}
