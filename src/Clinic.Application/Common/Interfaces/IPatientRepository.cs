namespace Clinic.Application.Common.Interfaces;

using Clinic.Domain.Entities;

/// <summary>
/// Repository contract for managing <see cref="Patient"/> entities.
/// </summary>
public interface IPatientRepository
{
    /// <summary>Retrieves a patient by identifier.</summary>
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Retrieves a patient by email address.</summary>
    Task<Patient?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>Retrieves all registered patients.</summary>
    Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Adds a new patient to the repository.</summary>
    Task AddAsync(Patient entity, CancellationToken ct = default);

    /// <summary>Marks an existing patient as modified.</summary>
    void Update(Patient entity);

    /// <summary>Removes a patient from the repository.</summary>
    void Remove(Patient entity);
}
