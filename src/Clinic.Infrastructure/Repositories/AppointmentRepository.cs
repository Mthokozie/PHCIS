namespace Clinic.Infrastructure.Repositories;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// EF Core implementation of <see cref="IAppointmentRepository"/>.
/// All read operations eager-load the Patient, Clinic, and TimeSlot navigations.
/// </summary>
public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    /// <summary>Initialises a new instance of <see cref="AppointmentRepository"/>.</summary>
    /// <param name="context">The application database context.</param>
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }

    private async Task<Guid> ResolvePatientIdAsync(Guid patientOrUserId, CancellationToken ct)
    {
        if (await Context.Patients.AnyAsync(p => p.Id == patientOrUserId, ct))
        {
            return patientOrUserId;
        }

        var userIdStr = patientOrUserId.ToString();
        var user = await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userIdStr, ct);

        if (user?.PatientId is Guid linkedPatientId)
        {
            return linkedPatientId;
        }

        return patientOrUserId;
    }

    /// <inheritdoc/>
    public override async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet
            .Include(a => a.Patient)
            .Include(a => a.Clinic)
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    /// <inheritdoc/>
    public async Task<Appointment?> GetByConfirmationCodeAsync(string confirmationCode, CancellationToken ct = default)
        => await DbSet
            .Include(a => a.Patient)
            .Include(a => a.Clinic)
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.ConfirmationCode == confirmationCode, ct);

    /// <inheritdoc/>
    public async Task<bool> HasPatientBookingOnSlotAsync(Guid patientId, Guid timeSlotId, CancellationToken ct = default)
    {
        var resolvedPatientId = await ResolvePatientIdAsync(patientId, ct);
        return await DbSet.AnyAsync(
            a => a.PatientId == resolvedPatientId
              && a.TimeSlotId == timeSlotId
              && a.Status != AppointmentStatus.Cancelled,
            ct);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken ct = default)
    {
        var resolvedPatientId = await ResolvePatientIdAsync(patientId, ct);
        return await DbSet
            .Where(a => a.PatientId == resolvedPatientId)
            .Include(a => a.Patient)
            .Include(a => a.Clinic)
            .Include(a => a.TimeSlot)
            .ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Appointment>> GetByClinicIdAsync(Guid clinicId, CancellationToken ct = default)
        => await DbSet
            .Where(a => a.ClinicId == clinicId)
            .Include(a => a.Patient)
            .Include(a => a.Clinic)
            .Include(a => a.TimeSlot)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public new async Task AddAsync(Appointment entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    /// <inheritdoc/>
    public new void Update(Appointment entity)
        => DbSet.Update(entity);
}
