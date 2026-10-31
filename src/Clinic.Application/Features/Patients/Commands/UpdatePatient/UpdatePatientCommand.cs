namespace Clinic.Application.Features.Patients.Commands.UpdatePatient;

using MediatR;

/// <summary>
/// Command to update an existing patient's contact details and optional profile image.
/// </summary>
public record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? ProfileImageUrl = null) : IRequest<bool>;
