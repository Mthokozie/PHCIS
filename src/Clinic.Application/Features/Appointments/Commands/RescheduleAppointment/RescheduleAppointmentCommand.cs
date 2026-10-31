namespace Clinic.Application.Features.Appointments.Commands.RescheduleAppointment;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command to reschedule an existing appointment to a new time slot.
/// </summary>
public record RescheduleAppointmentCommand(
    Guid AppointmentId,
    Guid NewTimeSlotId,
    string Reason) : IRequest<AppointmentConfirmationDto>;
