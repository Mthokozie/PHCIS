namespace Clinic.Application.Features.Patients.Commands.UpdatePatient;

using MediatR;

/// <summary>
/// Command to update an existing patient's contact details.
/// </summary>
public record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber) : IRequest<bool>;
