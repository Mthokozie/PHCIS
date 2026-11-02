namespace Clinic.Application.Common.Interfaces;

using Clinic.Domain.Entities;

/// <summary>
/// Repository contract for managing <see cref="ClinicStaff"/> entities.
/// </summary>
public interface IClinicStaffRepository
{
    /// <summary>Retrieves a clinic staff member by their unique identifier.</summary>
    Task<ClinicStaff?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Retrieves a clinic staff member by their email address.</summary>
    Task<ClinicStaff?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>Retrieves all staff members belonging to a specific clinic.</summary>
    Task<IReadOnlyList<ClinicStaff>> GetByClinicIdAsync(Guid clinicId, CancellationToken ct = default);

    /// <summary>Retrieves all staff members across all clinics.</summary>
    Task<IReadOnlyList<ClinicStaff>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Adds a new clinic staff member to the repository.</summary>
    Task AddAsync(ClinicStaff entity, CancellationToken ct = default);

    /// <summary>Marks an existing clinic staff member as modified.</summary>
    void Update(ClinicStaff entity);
}
