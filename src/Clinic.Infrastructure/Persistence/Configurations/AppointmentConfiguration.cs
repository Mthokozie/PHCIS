namespace Clinic.Infrastructure.Persistence.Configurations;

using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// EF Core entity type configuration for the <see cref="Appointment"/> entity.
/// Maps to the "Appointments" table.  All FK deletions are restricted to protect healthcare records.
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Reason)
            .HasMaxLength(500);

        builder.Property(a => a.ConfirmationCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(a => a.ConfirmationCode)
            .IsUnique();

        // Status enum is stored as int by EF Core convention; no extra conversion needed.
        builder.Property(a => a.Status)
            .HasConversion<int>();

        // All relationships use Restrict to preserve appointment history.
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Clinic)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TimeSlot)
            .WithMany(ts => ts.Appointments)
            .HasForeignKey(a => a.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
