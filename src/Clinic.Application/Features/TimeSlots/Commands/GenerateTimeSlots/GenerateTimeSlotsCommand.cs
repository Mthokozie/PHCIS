namespace Clinic.Application.Features.TimeSlots.Commands.GenerateTimeSlots;

using MediatR;

/// <summary>
/// Command to generate bookable time slots for a clinic on a given date.
/// </summary>
public record GenerateTimeSlotsCommand(
    Guid ClinicId,
    DateTime Date,
    int MaxCapacityPerSlot) : IRequest<int>;
