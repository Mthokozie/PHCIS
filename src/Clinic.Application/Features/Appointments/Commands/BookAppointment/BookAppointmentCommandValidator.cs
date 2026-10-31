namespace Clinic.Application.Features.Appointments.Commands.BookAppointment;

using FluentValidation;

/// <summary>
/// Validator for <see cref="BookAppointmentCommand"/>.
/// </summary>
public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    /// <summary>Initialises validation rules for booking an appointment.</summary>
    public BookAppointmentCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required.");
        RuleFor(x => x.TimeSlotId).NotEmpty().WithMessage("TimeSlotId is required.");
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
