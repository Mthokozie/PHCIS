namespace Clinic.Application.Features.Appointments.Commands.UpdateConsultationNotes;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command to update or record clinical notes during consultation.
/// </summary>
public record UpdateConsultationNotesCommand(Guid AppointmentId, Guid StaffId, string Notes) : IRequest<AppointmentDto>;
