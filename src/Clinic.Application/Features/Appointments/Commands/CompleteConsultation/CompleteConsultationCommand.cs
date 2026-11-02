namespace Clinic.Application.Features.Appointments.Commands.CompleteConsultation;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command for a clinic staff member to mark an appointment consultation as completed.
/// </summary>
public record CompleteConsultationCommand(
    Guid AppointmentId,
    Guid StaffId,
    string? Notes = null) : IRequest<AppointmentDto>;
