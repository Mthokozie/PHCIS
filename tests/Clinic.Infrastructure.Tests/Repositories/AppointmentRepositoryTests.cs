namespace Clinic.Infrastructure.Tests.Repositories;

using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Clinic.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AppointmentRepositoryTests
{
    private static ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task HasPatientBookingOnSlotAsync_WhenActiveBookingExists_ShouldReturnTrue()
    {
        using var context = CreateInMemoryContext();
        var repo = new AppointmentRepository(context);

        var patientId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var clinicId = Guid.NewGuid();

        var appointment = Appointment.Create(patientId, clinicId, slotId, "Test");
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var result = await repo.HasPatientBookingOnSlotAsync(patientId, slotId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasPatientBookingOnSlotAsync_WhenCancelledBookingExists_ShouldReturnFalse()
    {
        using var context = CreateInMemoryContext();
        var repo = new AppointmentRepository(context);

        var patientId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var clinicId = Guid.NewGuid();

        var appointment = Appointment.Create(patientId, clinicId, slotId, "Test");
        appointment.Cancel("No longer needed");
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var result = await repo.HasPatientBookingOnSlotAsync(patientId, slotId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistAppointment()
    {
        using var context = CreateInMemoryContext();
        var repo = new AppointmentRepository(context);

        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Test booking");
        await repo.AddAsync(appointment);
        await context.SaveChangesAsync();

        var saved = await context.Appointments.FindAsync(appointment.Id);
        saved.Should().NotBeNull();
        saved!.Reason.Should().Be("Test booking");
    }
}
