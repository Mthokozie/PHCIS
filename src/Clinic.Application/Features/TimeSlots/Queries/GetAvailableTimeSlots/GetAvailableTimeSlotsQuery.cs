namespace Clinic.Application.Features.TimeSlots.Queries.GetAvailableTimeSlots;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve available time slots for a clinic on a specified date.
/// </summary>
public record GetAvailableTimeSlotsQuery(Guid ClinicId, DateTime Date) : IRequest<IReadOnlyList<TimeSlotDto>>;
