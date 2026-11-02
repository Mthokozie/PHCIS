namespace Clinic.Application.Features.Appointments.Commands.ProcessAppointment;

using FluentValidation;

/// <summary>
/// Validator for <see cref="ProcessAppointmentCommand"/>.
/// </summary>
public class ProcessAppointmentCommandValidator : AbstractValidator<ProcessAppointmentCommand>
{
    /// <summary>Initialises validation rules.</summary>
    public ProcessAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment ID is required.");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required.");
    }
}
