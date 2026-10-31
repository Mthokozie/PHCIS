namespace Clinic.Domain.Tests.Entities;

using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class TimeSlotTests
{
    private static TimeSlot CreateSlot(int maxCapacity = 5)
        => TimeSlot.Create(Guid.NewGuid(), DateTime.Today.AddHours(9), DateTime.Today.AddHours(9).AddMinutes(30), maxCapacity);

    [Fact]
    public void Create_WithValidData_ShouldCreateTimeSlot()
    {
        var slot = CreateSlot();
        slot.Should().NotBeNull();
        slot.IsAvailable.Should().BeTrue();
        slot.CurrentBookings.Should().Be(0);
        slot.RemainingCapacity.Should().Be(5);
        slot.HasAvailableCapacity.Should().BeTrue();
    }

    [Fact]
    public void Reserve_WhenCapacityAvailable_ShouldIncrementBookings()
    {
        var slot = CreateSlot(2);
        slot.Reserve();
        slot.CurrentBookings.Should().Be(1);
        slot.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void Reserve_WhenAtMaxCapacity_ShouldMarkAsUnavailable()
    {
        var slot = CreateSlot(1);
        slot.Reserve();
        slot.IsAvailable.Should().BeFalse();
        slot.CurrentBookings.Should().Be(1);
    }

    [Fact]
    public void Reserve_WhenNoCapacity_ShouldThrowDomainException()
    {
        var slot = CreateSlot(1);
        slot.Reserve();
        var act = () => slot.Reserve();
        act.Should().Throw<DomainException>()
           .WithMessage("*capacity*");
    }

    [Fact]
    public void Release_ShouldDecrementBookingsAndRestoreAvailability()
    {
        var slot = CreateSlot(1);
        slot.Reserve();
        slot.IsAvailable.Should().BeFalse();
        slot.Release();
        slot.CurrentBookings.Should().Be(0);
        slot.IsAvailable.Should().BeTrue();
    }
}
