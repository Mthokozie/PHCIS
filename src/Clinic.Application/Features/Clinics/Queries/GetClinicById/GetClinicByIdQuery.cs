namespace Clinic.Application.Features.Clinics.Queries.GetClinicById;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve a single clinic by identifier.
/// </summary>
public record GetClinicByIdQuery(Guid Id) : IRequest<ClinicDto>;
