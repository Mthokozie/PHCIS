namespace Clinic.Application.Features.ClinicStaff.Commands.CreateClinicStaff;

using Clinic.Application.DTOs;
using Clinic.Domain.Enums;
using MediatR;

/// <summary>
/// Command to onboard and register a new internal clinic staff member.
/// </summary>
public record CreateClinicStaffCommand(
    Guid ClinicId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    StaffRole Role,
    string StaffRegistrationNumber) : IRequest<ClinicStaffDto>;
