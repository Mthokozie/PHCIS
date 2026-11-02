namespace Clinic.Application.Features.Appointments.Commands.AcceptAppointment;

using FluentValidation;

/// <summary>
/// Validator for <see cref="AcceptAppointmentCommand"/>.
/// </summary>
public class AcceptAppointmentCommandValidator : AbstractValidator<AcceptAppointmentCommand>
{
    /// <summary>Initialises validation rules.</summary>
    public AcceptAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment ID is required.");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required.");
    }
}
