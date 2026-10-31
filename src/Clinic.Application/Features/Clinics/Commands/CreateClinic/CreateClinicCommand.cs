namespace Clinic.Application.Features.Clinics.Commands.CreateClinic;

using MediatR;

/// <summary>
/// Command to register a new healthcare clinic.
/// </summary>
public record CreateClinicCommand(
    string Name,
    string Address,
    string Phone,
    string Email,
    string OpeningTime,
    string ClosingTime,
    int SlotDurationMinutes) : IRequest<Guid>;
