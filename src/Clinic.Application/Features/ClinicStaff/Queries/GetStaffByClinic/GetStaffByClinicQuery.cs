namespace Clinic.Application.Features.ClinicStaff.Queries.GetStaffByClinic;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve all internal staff members assigned to a clinic.
/// </summary>
public record GetStaffByClinicQuery(Guid ClinicId) : IRequest<IReadOnlyList<ClinicStaffDto>>;
