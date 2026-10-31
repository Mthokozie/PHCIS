namespace Clinic.Application.Common.Interfaces;

using Clinic.Domain.Entities;

/// <summary>
/// Repository contract for managing <see cref="Appointment"/> entities.
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>Retrieves an appointment by identifier with related details.</summary>
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Retrieves an appointment by its confirmation code.</summary>
    Task<Appointment?> GetByConfirmationCodeAsync(string confirmationCode, CancellationToken ct = default);

    /// <summary>Checks whether a patient already has a non-cancelled booking for the specified slot.</summary>
    Task<bool> HasPatientBookingOnSlotAsync(Guid patientId, Guid timeSlotId, CancellationToken ct = default);

    /// <summary>Retrieves all appointments for a specific patient.</summary>
    Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken ct = default);

    /// <summary>Retrieves all appointments for a specific clinic.</summary>
    Task<IReadOnlyList<Appointment>> GetByClinicIdAsync(Guid clinicId, CancellationToken ct = default);

    /// <summary>Adds a new appointment to the repository.</summary>
    Task AddAsync(Appointment entity, CancellationToken ct = default);

    /// <summary>Marks an existing appointment as modified.</summary>
    void Update(Appointment entity);
}
