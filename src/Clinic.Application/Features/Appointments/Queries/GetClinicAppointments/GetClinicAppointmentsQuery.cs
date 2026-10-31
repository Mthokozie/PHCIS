namespace Clinic.Application.Features.Appointments.Queries.GetClinicAppointments;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve all appointments for a specific clinic.
/// </summary>
public record GetClinicAppointmentsQuery(Guid ClinicId) : IRequest<IReadOnlyList<AppointmentDto>>;
