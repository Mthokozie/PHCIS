namespace Clinic.Application.Features.ClinicStaff.Queries.GetClinicStaffById;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Query to retrieve a single clinic staff member by their unique identifier.
/// </summary>
public record GetClinicStaffByIdQuery(Guid StaffId) : IRequest<ClinicStaffDto>;
