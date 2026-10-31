using Clinic.Domain.Common;

namespace Clinic.Domain.Events;

/// <summary>
/// Domain event raised when a new appointment is successfully booked.
/// </summary>
/// <param name="AppointmentId">The unique identifier of the newly created appointment.</param>
/// <param name="PatientId">The unique identifier of the patient who booked the appointment.</param>
/// <param name="ClinicId">The unique identifier of the clinic at which the appointment was booked.</param>
/// <param name="ConfirmationCode">The human-readable confirmation code generated for the appointment.</param>
public record AppointmentBookedEvent(
    Guid AppointmentId,
    Guid PatientId,
    Guid ClinicId,
    string ConfirmationCode) : IDomainEvent;
