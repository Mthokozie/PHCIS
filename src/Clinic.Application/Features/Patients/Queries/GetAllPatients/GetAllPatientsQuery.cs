namespace Clinic.Application.Features.Patients.Queries.GetAllPatients;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve all registered patients.
/// </summary>
public record GetAllPatientsQuery : IRequest<IReadOnlyList<PatientDto>>;
