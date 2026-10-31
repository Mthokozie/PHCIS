namespace Clinic.Application.Tests.Features.Appointments;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Application.Features.Appointments.Commands.BookAppointment;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

public class BookAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepo;
    private readonly Mock<ITimeSlotRepository> _mockTimeSlotRepo;
    private readonly Mock<IPatientRepository> _mockPatientRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookAppointmentCommandHandler _handler;

    public BookAppointmentCommandHandlerTests()
    {
        _mockAppointmentRepo = new Mock<IAppointmentRepository>();
        _mockTimeSlotRepo = new Mock<ITimeSlotRepository>();
        _mockPatientRepo = new Mock<IPatientRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _handler = new BookAppointmentCommandHandler(
            _mockAppointmentRepo.Object,
            _mockTimeSlotRepo.Object,
            _mockPatientRepo.Object,
            _mockUnitOfWork.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidBooking_ShouldCreateAppointmentAndReturnConfirmation()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var clinicId = Guid.NewGuid();

        var patient = Patient.Create("John", "Doe", "john@test.com", "+2782", new DateTime(1990, 1, 1), "ID001");
        var slot = TimeSlot.Create(clinicId, DateTime.Today.AddHours(10), DateTime.Today.AddHours(10).AddMinutes(30), 5);

        _mockPatientRepo.Setup(r => r.GetByIdAsync(patientId, default))
            .ReturnsAsync(patient);
        _mockTimeSlotRepo.Setup(r => r.GetByIdAsync(slotId, default))
            .ReturnsAsync(slot);
        _mockAppointmentRepo.Setup(r => r.HasPatientBookingOnSlotAsync(patientId, slotId, default))
            .ReturnsAsync(false);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var expectedConfirmation = new AppointmentConfirmationDto
        {
            ConfirmationCode = "PHCIS-TESTCODE",
            PatientName = "John Doe",
            Message = "Booking confirmed!"
        };
        _mockMapper.Setup(m => m.Map<AppointmentConfirmationDto>(It.IsAny<Appointment>()))
            .Returns(expectedConfirmation);

        var command = new BookAppointmentCommand(patientId, slotId, "General checkup");

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.ConfirmationCode.Should().Be("PHCIS-TESTCODE");
        _mockAppointmentRepo.Verify(r => r.AddAsync(It.IsAny<Appointment>(), default), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_PatientNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _mockPatientRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Patient?)null);

        var command = new BookAppointmentCommand(Guid.NewGuid(), Guid.NewGuid(), "Checkup");

        // Act
        var act = () => _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_SlotNotAvailable_ShouldThrowDomainException()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var patient = Patient.Create("John", "Doe", "john@test.com", "+2782", new DateTime(1990, 1, 1), "ID001");

        var slot = TimeSlot.Create(Guid.NewGuid(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(10).AddMinutes(30), 1);
        slot.Reserve();

        _mockPatientRepo.Setup(r => r.GetByIdAsync(patientId, default)).ReturnsAsync(patient);
        _mockTimeSlotRepo.Setup(r => r.GetByIdAsync(slotId, default)).ReturnsAsync(slot);

        var command = new BookAppointmentCommand(patientId, slotId, "Checkup");

        // Act
        var act = () => _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_DoubleBooking_ShouldThrowDomainException()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var slotId = Guid.NewGuid();
        var patient = Patient.Create("John", "Doe", "john@test.com", "+2782", new DateTime(1990, 1, 1), "ID001");
        var slot = TimeSlot.Create(Guid.NewGuid(), DateTime.Today.AddHours(10), DateTime.Today.AddHours(10).AddMinutes(30), 5);

        _mockPatientRepo.Setup(r => r.GetByIdAsync(patientId, default)).ReturnsAsync(patient);
        _mockTimeSlotRepo.Setup(r => r.GetByIdAsync(slotId, default)).ReturnsAsync(slot);
        _mockAppointmentRepo.Setup(r => r.HasPatientBookingOnSlotAsync(patientId, slotId, default))
            .ReturnsAsync(true);

        var command = new BookAppointmentCommand(patientId, slotId, "Checkup");

        // Act
        var act = () => _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already*");
    }
}
