namespace Clinic.Application.Features.TimeSlots.Commands.GenerateTimeSlots;

using FluentValidation;

/// <summary>
/// Validator for <see cref="GenerateTimeSlotsCommand"/>.
/// </summary>
public class GenerateTimeSlotsCommandValidator : AbstractValidator<GenerateTimeSlotsCommand>
{
    /// <summary>Initialises validation rules for generating time slots.</summary>
    public GenerateTimeSlotsCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Cannot generate time slots for a past date.");
        RuleFor(x => x.MaxCapacityPerSlot).GreaterThan(0).LessThanOrEqualTo(100);
    }
}
