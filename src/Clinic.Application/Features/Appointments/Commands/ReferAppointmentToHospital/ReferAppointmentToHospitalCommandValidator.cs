namespace Clinic.Application.Features.Appointments.Commands.ReferAppointmentToHospital;

using FluentValidation;

/// <summary>
/// Validator for <see cref="ReferAppointmentToHospitalCommand"/>.
/// </summary>
public class ReferAppointmentToHospitalCommandValidator : AbstractValidator<ReferAppointmentToHospitalCommand>
{
    /// <summary>Initialises validation rules.</summary>
    public ReferAppointmentToHospitalCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment ID is required.");

        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff ID is required.");

        RuleFor(x => x.HospitalName)
            .NotEmpty().WithMessage("Referral hospital name is required.")
            .MaximumLength(200).WithMessage("Hospital name must not exceed 200 characters.");

        RuleFor(x => x.ReferralReason)
            .NotEmpty().WithMessage("Referral reason is required.")
            .MaximumLength(1000).WithMessage("Referral reason must not exceed 1000 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.");
    }
}
