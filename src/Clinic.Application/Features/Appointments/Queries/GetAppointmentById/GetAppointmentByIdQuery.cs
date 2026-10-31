namespace Clinic.Application.Features.Appointments.Queries.GetAppointmentById;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve a single appointment by identifier.
/// </summary>
public record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto>;
