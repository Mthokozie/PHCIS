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
        var firstName = "Ntokozo";
        var lastName = "Ngwenya";
        var email = "Ntokozo.Ngwenya@test.com";
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
        patient.FullName.Should().Be("Ntokozo Ngwenya");
    }

    [Theory]
    [InlineData("", "Ngwenya", "Ntokozo@test.com", "+2782")]
    [InlineData("Ntokozo", "", "Ntokozo@test.com", "+2782")]
    [InlineData("Ntokozo", "Ngwenya", "", "+2782")]
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
        var patient = Patient.Create("Swazi", "Zulu", "Swazi@test.com", "+2783", DateTime.UtcNow.AddYears(-25), "ID456");
        patient.Deactivate();
        patient.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldUpdateNameAndPhone()
    {
        var patient = Patient.Create("Swazi", "Zulu", "Swazi@test.com", "+2783", DateTime.UtcNow.AddYears(-25), "ID456");
        patient.Update("Swazit", "Jobe", "+2789999999");
        patient.FirstName.Should().Be("Swazit");
        patient.LastName.Should().Be("Jobe");
        patient.PhoneNumber.Should().Be("+2789999999");
    }
}
