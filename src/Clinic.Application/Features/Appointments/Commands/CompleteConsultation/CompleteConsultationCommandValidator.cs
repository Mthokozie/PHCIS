namespace Clinic.Application.Features.Appointments.Commands.CompleteConsultation;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CompleteConsultationCommand"/>.
/// </summary>
public class CompleteConsultationCommandValidator : AbstractValidator<CompleteConsultationCommand>
{
    /// <summary>Initialises validation rules.</summary>
    public CompleteConsultationCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment ID is required.");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.");
    }
}
