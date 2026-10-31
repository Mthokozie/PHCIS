namespace Clinic.Infrastructure.Repositories;

using Clinic.Application.Common.Interfaces;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core implementation of <see cref="IClinicRepository"/>.
/// Provides clinic-specific query operations on top of <see cref="BaseRepository{T}"/>.
/// </summary>
public class ClinicRepository : BaseRepository<global::Clinic.Domain.Entities.Clinic>, IClinicRepository
{
    /// <summary>Initialises a new instance of <see cref="ClinicRepository"/>.</summary>
    /// <param name="context">The application database context.</param>
    public ClinicRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public override async Task<global::Clinic.Domain.Entities.Clinic?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object[] { id }, ct);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<global::Clinic.Domain.Entities.Clinic>> GetAllActiveAsync(CancellationToken ct = default)
        => await DbSet.Where(c => c.IsActive).ToListAsync(ct);

    /// <inheritdoc/>
    public new async Task AddAsync(global::Clinic.Domain.Entities.Clinic entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    /// <inheritdoc/>
    public new void Update(global::Clinic.Domain.Entities.Clinic entity)
        => DbSet.Update(entity);
}
