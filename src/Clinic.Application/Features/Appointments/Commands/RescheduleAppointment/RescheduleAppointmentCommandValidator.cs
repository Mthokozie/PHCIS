namespace Clinic.Application.Features.Appointments.Commands.RescheduleAppointment;

using FluentValidation;

/// <summary>
/// Validator for <see cref="RescheduleAppointmentCommand"/>.
/// </summary>
public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
{
    /// <summary>Initialises validation rules for rescheduling an appointment.</summary>
    public RescheduleAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty();
        RuleFor(x => x.NewTimeSlotId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
