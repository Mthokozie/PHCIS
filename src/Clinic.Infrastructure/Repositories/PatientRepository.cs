namespace Clinic.Infrastructure.Repositories;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core implementation of <see cref="IPatientRepository"/>.
/// Provides patient-specific query operations on top of <see cref="BaseRepository{T}"/>.
/// </summary>
public class PatientRepository : BaseRepository<Patient>, IPatientRepository
{
    /// <summary>Initialises a new instance of <see cref="PatientRepository"/>.</summary>
    /// <param name="context">The application database context.</param>
    public PatientRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public override async Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object[] { id }, ct);

    /// <inheritdoc/>
    public async Task<Patient?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(p => p.Email == email, ct);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Patient>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.ToListAsync(ct);

    /// <inheritdoc/>
    public new async Task AddAsync(Patient entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    /// <inheritdoc/>
    public new void Update(Patient entity)
        => DbSet.Update(entity);

    /// <inheritdoc/>
    public new void Remove(Patient entity)
        => DbSet.Remove(entity);
}
