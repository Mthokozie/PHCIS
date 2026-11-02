namespace Clinic.Application.Features.Appointments.Commands.UpdateConsultationNotes;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateConsultationNotesCommand"/>.
/// </summary>
public class UpdateConsultationNotesCommandValidator : AbstractValidator<UpdateConsultationNotesCommand>
{
    public UpdateConsultationNotesCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment ID is required.");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required.");

        RuleFor(x => x.Notes)
            .NotNull().WithMessage("Consultation notes cannot be null.")
            .MaximumLength(4000).WithMessage("Consultation notes cannot exceed 4000 characters.");
    }
}
