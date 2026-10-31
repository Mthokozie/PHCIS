namespace Clinic.Application.Features.Appointments.Queries.GetPatientAppointments;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve all appointments for a specific patient.
/// </summary>
public record GetPatientAppointmentsQuery(Guid PatientId) : IRequest<IReadOnlyList<AppointmentDto>>;
