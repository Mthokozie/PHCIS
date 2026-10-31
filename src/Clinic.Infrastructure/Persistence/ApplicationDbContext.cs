namespace Clinic.Infrastructure.Persistence;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Main EF Core database context implementing <see cref="IApplicationDbContext"/> and <see cref="IUnitOfWork"/>.
/// Extends <see cref="IdentityDbContext{TUser}"/> for ASP.NET Identity support.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext, IUnitOfWork
{
    /// <summary>Initialises a new instance of <see cref="ApplicationDbContext"/>.</summary>
    /// <param name="options">EF Core context options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <inheritdoc/>
    public DbSet<Patient> Patients => Set<Patient>();

    /// <inheritdoc/>
    public DbSet<global::Clinic.Domain.Entities.Clinic> Clinics => Set<global::Clinic.Domain.Entities.Clinic>();

    /// <inheritdoc/>
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();

    /// <inheritdoc/>
    public DbSet<Appointment> Appointments => Set<Appointment>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all IEntityTypeConfiguration<T> classes in this assembly automatically.
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    /// <summary>
    /// Saves all pending changes and automatically stamps <see cref="Clinic.Domain.Common.BaseEntity"/>
    /// <c>CreatedAt</c> / <c>UpdatedAt</c> audit fields.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that the operation should be cancelled.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<global::Clinic.Domain.Common.BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
