namespace Clinic.Application.Common.Interfaces;

/// <summary>
/// Repository contract for managing <see cref="Clinic.Domain.Entities.Clinic"/> entities.
/// </summary>
public interface IClinicRepository
{
    /// <summary>Retrieves a clinic by identifier.</summary>
    Task<global::Clinic.Domain.Entities.Clinic?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Retrieves all active clinics.</summary>
    Task<IReadOnlyList<global::Clinic.Domain.Entities.Clinic>> GetAllActiveAsync(CancellationToken ct = default);

    /// <summary>Adds a new clinic to the repository.</summary>
    Task AddAsync(global::Clinic.Domain.Entities.Clinic entity, CancellationToken ct = default);

    /// <summary>Marks an existing clinic as modified.</summary>
    void Update(global::Clinic.Domain.Entities.Clinic entity);
}
