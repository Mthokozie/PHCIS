namespace Clinic.Application.Common.Interfaces;

using Clinic.Domain.Entities;

/// <summary>
/// Repository contract for managing <see cref="TimeSlot"/> entities.
/// </summary>
public interface ITimeSlotRepository
{
    /// <summary>Retrieves a time slot by identifier.</summary>
    Task<TimeSlot?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Retrieves available time slots for a specific clinic and date.</summary>
    Task<IReadOnlyList<TimeSlot>> GetAvailableSlotsByClinicAndDateAsync(
        Guid clinicId,
        DateOnly date,
        CancellationToken ct = default);

    /// <summary>Adds a batch of time slots to the repository.</summary>
    Task AddRangeAsync(IEnumerable<TimeSlot> slots, CancellationToken ct = default);

    /// <summary>Marks an existing time slot as modified.</summary>
    void Update(TimeSlot entity);
}
