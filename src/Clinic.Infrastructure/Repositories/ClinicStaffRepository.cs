namespace Clinic.Infrastructure.Repositories;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core implementation of <see cref="IClinicStaffRepository"/>.
/// </summary>
public class ClinicStaffRepository : BaseRepository<ClinicStaff>, IClinicStaffRepository
{
    /// <summary>Initialises a new instance of <see cref="ClinicStaffRepository"/>.</summary>
    /// <param name="context">The application database context.</param>
    public ClinicStaffRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public override async Task<ClinicStaff?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet
            .Include(s => s.Clinic)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    /// <inheritdoc/>
    public async Task<ClinicStaff?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await DbSet
            .Include(s => s.Clinic)
            .FirstOrDefaultAsync(s => s.Email == normalizedEmail, ct);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ClinicStaff>> GetByClinicIdAsync(Guid clinicId, CancellationToken ct = default)
        => await DbSet
            .Where(s => s.ClinicId == clinicId)
            .Include(s => s.Clinic)
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ClinicStaff>> GetAllAsync(CancellationToken ct = default)
        => await DbSet
            .Include(s => s.Clinic)
            .OrderBy(s => s.Clinic.Name)
            .ThenBy(s => s.LastName)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public new async Task AddAsync(ClinicStaff entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    /// <inheritdoc/>
    public new void Update(ClinicStaff entity)
        => DbSet.Update(entity);
}
