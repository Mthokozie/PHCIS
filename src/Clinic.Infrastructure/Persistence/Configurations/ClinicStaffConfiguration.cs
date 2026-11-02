namespace Clinic.Infrastructure.Persistence.Configurations;

using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// EF Core entity type configuration for the <see cref="ClinicStaff"/> entity.
/// Maps to the "ClinicStaff" table.
/// </summary>
public class ClinicStaffConfiguration : IEntityTypeConfiguration<ClinicStaff>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ClinicStaff> builder)
    {
        builder.ToTable("ClinicStaff");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.StaffRegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.StaffRegistrationNumber);

        builder.Property(s => s.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.IsActive)
            .IsRequired();

        // One Clinic has many ClinicStaff members
        builder.HasOne(s => s.Clinic)
            .WithMany(c => c.Staff)
            .HasForeignKey(s => s.ClinicId)
            .OnDelete(DeleteBehavior.Restrict);

        // One ClinicStaff can process many Appointments
        builder.HasMany(s => s.ProcessedAppointments)
            .WithOne(a => a.AttendingStaff)
            .HasForeignKey(a => a.AttendingStaffId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
