namespace Clinic.Application.Features.Appointments.Commands.AcceptAppointment;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command for a clinic staff member to accept and claim an incoming appointment.
/// </summary>
public record AcceptAppointmentCommand(
    Guid AppointmentId,
    Guid StaffId) : IRequest<AppointmentDto>;
