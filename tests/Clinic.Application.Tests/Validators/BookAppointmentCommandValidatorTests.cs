namespace Clinic.Application.Tests.Validators;

using Clinic.Application.Features.Appointments.Commands.BookAppointment;
using FluentAssertions;
using Xunit;

public class BookAppointmentCommandValidatorTests
{
    private readonly BookAppointmentCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        var command = new BookAppointmentCommand(Guid.NewGuid(), Guid.NewGuid(), "General checkup");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyPatientId_ShouldFail()
    {
        var command = new BookAppointmentCommand(Guid.Empty, Guid.NewGuid(), "Checkup");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PatientId");
    }

    [Fact]
    public void Validate_EmptyTimeSlotId_ShouldFail()
    {
        var command = new BookAppointmentCommand(Guid.NewGuid(), Guid.Empty, "Checkup");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TimeSlotId");
    }

    [Fact]
    public void Validate_EmptyReason_ShouldFail()
    {
        var command = new BookAppointmentCommand(Guid.NewGuid(), Guid.NewGuid(), "");
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Reason");
    }

    [Fact]
    public void Validate_ReasonTooLong_ShouldFail()
    {
        var command = new BookAppointmentCommand(Guid.NewGuid(), Guid.NewGuid(), new string('x', 501));
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}
