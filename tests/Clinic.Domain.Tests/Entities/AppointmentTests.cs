namespace Clinic.Domain.Tests.Entities;

using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class AppointmentTests
{
    [Fact]
    public void Create_ShouldGenerateConfirmationCode()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "General checkup");
        appointment.ConfirmationCode.Should().StartWith("PHCIS-");
        appointment.ConfirmationCode.Length.Should().BeGreaterThan(6);
        appointment.Status.Should().Be(AppointmentStatus.Pending);
    }

    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Checkup");
        appointment.Confirm();
        appointment.Status.Should().Be(AppointmentStatus.Confirmed);
    }

    [Fact]
    public void Cancel_WithReason_ShouldSetStatusAndTimestamp()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Checkup");
        appointment.Cancel("Patient unavailable");
        appointment.Status.Should().Be(AppointmentStatus.Cancelled);
        appointment.CancellationReason.Should().Be("Patient unavailable");
        appointment.CancelledAt.Should().NotBeNull();
    }

    [Fact]
    public void Cancel_AlreadyCancelled_ShouldThrowDomainException()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Checkup");
        appointment.Cancel("First cancel");
        var act = () => appointment.Cancel("Second cancel");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Complete_ShouldSetStatusToCompleted()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Checkup");
        appointment.Confirm();
        appointment.Complete();
        appointment.Status.Should().Be(AppointmentStatus.Completed);
    }
}
