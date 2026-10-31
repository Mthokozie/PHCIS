namespace Clinic.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// EF Core entity type configuration for the <see cref="Clinic.Domain.Entities.Clinic"/> entity.
/// Maps to the "Clinics" table with OpeningTime / ClosingTime stored as strings.
/// </summary>
public class ClinicConfiguration : IEntityTypeConfiguration<global::Clinic.Domain.Entities.Clinic>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<global::Clinic.Domain.Entities.Clinic> builder)
    {
        builder.ToTable("Clinics");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);

        // Store TimeSpan as a string (e.g. "08:00:00") for broad SQL Server compatibility.
        builder.Property(c => c.OpeningTime)
            .HasConversion<string>();

        builder.Property(c => c.ClosingTime)
            .HasConversion<string>();

        // Cascade delete TimeSlots when a clinic is removed.
        builder.HasMany(c => c.TimeSlots)
            .WithOne(ts => ts.Clinic)
            .HasForeignKey(ts => ts.ClinicId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict delete on Appointments to protect healthcare records.
        builder.HasMany(c => c.Appointments)
            .WithOne(a => a.Clinic)
            .HasForeignKey(a => a.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
