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
    {
        var patient = await DbSet.FindAsync(new object[] { id }, ct);
        if (patient is not null)
        {
            return patient;
        }

        // Fallback: if the caller passed the ASP.NET Identity ApplicationUser.Id (JWT 'sub' claim)
        // instead of the PatientId claim, resolve the linked Patient automatically.
        var userIdStr = id.ToString();
        var user = await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userIdStr, ct);

        if (user is not null)
        {
            if (user.PatientId.HasValue)
            {
                patient = await DbSet.FindAsync(new object[] { user.PatientId.Value }, ct);
                if (patient is not null)
                {
                    return patient;
                }
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                return await DbSet.FirstOrDefaultAsync(p => p.Email == user.Email, ct);
            }
        }

        return null;
    }

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
