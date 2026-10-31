namespace Clinic.Domain.Tests.Entities;

using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class PatientTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreatePatient()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@test.com";
        var phone = "+27821234567";
        var dob = new DateTime(1990, 1, 15);
        var idNumber = "9001150123456";

        // Act
        var patient = Patient.Create(firstName, lastName, email, phone, dob, idNumber);

        // Assert
        patient.Should().NotBeNull();
        patient.FirstName.Should().Be(firstName);
        patient.LastName.Should().Be(lastName);
        patient.Email.Should().Be(email);
        patient.PhoneNumber.Should().Be(phone);
        patient.IsActive.Should().BeTrue();
        patient.FullName.Should().Be("John Doe");
    }

    [Theory]
    [InlineData("", "Doe", "john@test.com", "+2782")]
    [InlineData("John", "", "john@test.com", "+2782")]
    [InlineData("John", "Doe", "", "+2782")]
    public void Create_WithMissingRequiredFields_ShouldThrowDomainException(
        string firstName, string lastName, string email, string phone)
    {
        // Act
        var act = () => Patient.Create(firstName, lastName, email, phone, DateTime.UtcNow.AddYears(-20), "ID123");

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var patient = Patient.Create("Jane", "Smith", "jane@test.com", "+2783", DateTime.UtcNow.AddYears(-25), "ID456");
        patient.Deactivate();
        patient.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldUpdateNameAndPhone()
    {
        var patient = Patient.Create("Jane", "Smith", "jane@test.com", "+2783", DateTime.UtcNow.AddYears(-25), "ID456");
        patient.Update("Janet", "Jones", "+2789999999");
        patient.FirstName.Should().Be("Janet");
        patient.LastName.Should().Be("Jones");
        patient.PhoneNumber.Should().Be("+2789999999");
    }
}
