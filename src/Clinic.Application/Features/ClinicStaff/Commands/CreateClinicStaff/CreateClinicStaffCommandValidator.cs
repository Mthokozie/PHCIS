namespace Clinic.Application.Features.ClinicStaff.Commands.CreateClinicStaff;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateClinicStaffCommand"/>.
/// </summary>
public class CreateClinicStaffCommandValidator : AbstractValidator<CreateClinicStaffCommand>
{
    /// <summary>Initialises validation rules.</summary>
    public CreateClinicStaffCommandValidator()
    {
        RuleFor(x => x.ClinicId)
            .NotEmpty().WithMessage("Clinic ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.StaffRegistrationNumber)
            .NotEmpty().WithMessage("Staff registration number is required.")
            .MaximumLength(50).WithMessage("Staff registration number must not exceed 50 characters.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("A valid staff role must be specified.");
    }
}
