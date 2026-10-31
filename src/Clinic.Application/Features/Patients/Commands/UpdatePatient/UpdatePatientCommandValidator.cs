namespace Clinic.Application.Features.Patients.Commands.UpdatePatient;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdatePatientCommand"/>.
/// </summary>
public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    /// <summary>Initialises validation rules for updating a patient.</summary>
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
    }
}
