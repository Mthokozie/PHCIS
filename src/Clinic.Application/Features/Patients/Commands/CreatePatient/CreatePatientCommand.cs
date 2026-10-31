namespace Clinic.Application.Features.Patients.Commands.CreatePatient;

using MediatR;

/// <summary>
/// Command to register a new patient in the system.
/// </summary>
public record CreatePatientCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    string IdNumber) : IRequest<Guid>;
