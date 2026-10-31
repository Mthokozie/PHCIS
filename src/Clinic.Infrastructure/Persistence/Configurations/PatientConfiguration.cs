namespace Clinic.Infrastructure.Persistence.Configurations;

using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// EF Core entity type configuration for the <see cref="Patient"/> entity.
/// Maps to the "Patients" table and enforces business constraints.
/// </summary>
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.IdNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.IdNumber)
            .IsUnique();

        // Restrict delete to preserve healthcare data integrity.
        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
