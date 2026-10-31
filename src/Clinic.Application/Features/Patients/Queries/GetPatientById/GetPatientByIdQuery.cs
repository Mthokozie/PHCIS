namespace Clinic.Application.Features.Patients.Queries.GetPatientById;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve a single patient by identifier.
/// </summary>
public record GetPatientByIdQuery(Guid Id) : IRequest<PatientDto?>;
