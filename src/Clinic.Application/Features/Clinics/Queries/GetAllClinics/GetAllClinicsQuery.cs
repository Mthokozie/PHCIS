namespace Clinic.Application.Features.Clinics.Queries.GetAllClinics;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve all active clinics.
/// </summary>
public record GetAllClinicsQuery : IRequest<IReadOnlyList<ClinicDto>>;
