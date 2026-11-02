namespace Clinic.Application.Features.Appointments.Commands.ProcessAppointment;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command for a clinic staff member to start processing an appointment / begin consultation.
/// </summary>
public record ProcessAppointmentCommand(
    Guid AppointmentId,
    Guid StaffId) : IRequest<AppointmentDto>;
