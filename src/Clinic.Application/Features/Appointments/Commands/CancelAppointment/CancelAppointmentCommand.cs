namespace Clinic.Application.Features.Appointments.Commands.CancelAppointment;

using MediatR;

/// <summary>
/// Command to cancel an existing appointment and release its reserved slot capacity.
/// </summary>
public record CancelAppointmentCommand(
    Guid AppointmentId,
    string Reason) : IRequest<bool>;
