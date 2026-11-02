namespace Clinic.Application.Features.Appointments.Commands.ReferAppointmentToHospital;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command for a clinic staff member to refer a consultation to a district/regional hospital.
/// </summary>
public record ReferAppointmentToHospitalCommand(
    Guid AppointmentId,
    Guid StaffId,
    string HospitalName,
    string ReferralReason,
    string? Notes = null) : IRequest<AppointmentDto>;
