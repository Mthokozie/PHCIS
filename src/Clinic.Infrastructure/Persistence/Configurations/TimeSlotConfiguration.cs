namespace Clinic.Infrastructure.Persistence.Configurations;

using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// EF Core entity type configuration for the <see cref="TimeSlot"/> entity.
/// Maps to the "TimeSlots" table with a composite unique index on (ClinicId, StartTime).
/// </summary>
public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.ToTable("TimeSlots");

        builder.HasKey(ts => ts.Id);

        // Composite unique index to prevent duplicate slots per clinic.
        builder.HasIndex(ts => new { ts.ClinicId, ts.StartTime })
            .IsUnique();

        // Clinic relationship is configured from ClinicConfiguration (cascade).
        // Re-declared here for explicit FK definition.
        builder.HasOne(ts => ts.Clinic)
            .WithMany(c => c.TimeSlots)
            .HasForeignKey(ts => ts.ClinicId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict delete on Appointments to preserve booking history.
        builder.HasMany(ts => ts.Appointments)
            .WithOne(a => a.TimeSlot)
            .HasForeignKey(a => a.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
