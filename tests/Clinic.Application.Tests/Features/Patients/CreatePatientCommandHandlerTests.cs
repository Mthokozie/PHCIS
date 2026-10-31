namespace Clinic.Application.Tests.Features.Patients;

using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Patients.Commands.CreatePatient;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

public class CreatePatientCommandHandlerTests
{
    private readonly Mock<IPatientRepository> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly CreatePatientCommandHandler _handler;

    public CreatePatientCommandHandlerTests()
    {
        _mockRepo = new Mock<IPatientRepository>();
        _mockUow = new Mock<IUnitOfWork>();
        _handler = new CreatePatientCommandHandler(_mockRepo.Object, _mockUow.Object);
    }

    [Fact]
    public async Task Handle_NewPatient_ShouldAddAndReturnId()
    {
        _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync((Patient?)null);
        _mockUow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var command = new CreatePatientCommand(
            "Alice", "Smith", "alice@test.com", "+2781234567",
            new DateTime(1985, 5, 20), "8505200123456");

        var result = await _handler.Handle(command, default);

        result.Should().NotBe(Guid.Empty);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Patient>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowDomainException()
    {
        var existing = Patient.Create("Bob", "Jones", "alice@test.com", "+2781", new DateTime(1980, 1, 1), "ID999");
        _mockRepo.Setup(r => r.GetByEmailAsync("alice@test.com", default))
            .ReturnsAsync(existing);

        var command = new CreatePatientCommand(
            "Alice", "Smith", "alice@test.com", "+2781234567",
            new DateTime(1985, 5, 20), "8505200123456");

        var act = () => _handler.Handle(command, default);
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*email*");
    }
}
