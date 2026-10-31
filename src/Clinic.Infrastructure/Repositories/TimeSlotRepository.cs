namespace Clinic.Infrastructure.Repositories;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core implementation of <see cref="ITimeSlotRepository"/>.
/// Provides time-slot-specific query operations on top of <see cref="BaseRepository{T}"/>.
/// </summary>
public class TimeSlotRepository : BaseRepository<TimeSlot>, ITimeSlotRepository
{
    /// <summary>Initialises a new instance of <see cref="TimeSlotRepository"/>.</summary>
    /// <param name="context">The application database context.</param>
    public TimeSlotRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public override async Task<TimeSlot?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object[] { id }, ct);

    /// <inheritdoc/>
    /// <remarks>
    /// Filters by clinic, date, availability flag, and remaining capacity.
    /// Results are ordered by <see cref="TimeSlot.StartTime"/> ascending and the
    /// related <see cref="Clinic.Domain.Entities.Clinic"/> navigation is eager-loaded.
    /// </remarks>
    public async Task<IReadOnlyList<TimeSlot>> GetAvailableSlotsByClinicAndDateAsync(
        Guid clinicId,
        DateOnly date,
        CancellationToken ct = default)
        => await DbSet
            .Where(ts => ts.ClinicId == clinicId
                      && ts.StartTime.Date == date.ToDateTime(TimeOnly.MinValue).Date
                      && ts.IsAvailable
                      && ts.CurrentBookings < ts.MaxCapacity)
            .OrderBy(ts => ts.StartTime)
            .Include(ts => ts.Clinic)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<TimeSlot> slots, CancellationToken ct = default)
        => await DbSet.AddRangeAsync(slots, ct);

    /// <inheritdoc/>
    public new void Update(TimeSlot entity)
        => DbSet.Update(entity);
}
