namespace Clinic.Application.Features.Appointments.Commands.CancelAppointment;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CancelAppointmentCommand"/>.
/// </summary>
public class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
{
    /// <summary>Initialises validation rules for cancelling an appointment.</summary>
    public CancelAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
