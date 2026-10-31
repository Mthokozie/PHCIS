namespace Clinic.Application.Common.Interfaces;

using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Abstraction over the application's database context.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>Gets the Patients set.</summary>
    DbSet<Patient> Patients { get; }

    /// <summary>Gets the Clinics set.</summary>
    DbSet<global::Clinic.Domain.Entities.Clinic> Clinics { get; }

    /// <summary>Gets the TimeSlots set.</summary>
    DbSet<TimeSlot> TimeSlots { get; }

    /// <summary>Gets the Appointments set.</summary>
    DbSet<Appointment> Appointments { get; }

    /// <summary>Persists all pending changes asynchronously.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
