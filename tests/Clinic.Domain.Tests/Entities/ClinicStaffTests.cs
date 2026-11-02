namespace Clinic.Domain.Tests.Entities;

using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class ClinicStaffTests
{
    [Fact]
    public void Create_WithValidParameters_ReturnsActiveStaff()
    {
        // Arrange
        var clinicId = Guid.NewGuid();

        // Act
        var staff = ClinicStaff.Create(
            clinicId,
            "Nomvula",
            "Mthembu",
            "nomvula.mthembu@kznhealth.gov.za",
            "+27 35 572 1042",
            StaffRole.Nurse,
            "SANC-2024-9871");

        // Assert
        staff.Should().NotBeNull();
        staff.Id.Should().NotBeEmpty();
        staff.ClinicId.Should().Be(clinicId);
        staff.FirstName.Should().Be("Nomvula");
        staff.LastName.Should().Be("Mthembu");
        staff.FullName.Should().Be("Nomvula Mthembu");
        staff.Email.Should().Be("nomvula.mthembu@kznhealth.gov.za");
        staff.PhoneNumber.Should().Be("+27 35 572 1042");
        staff.Role.Should().Be(StaffRole.Nurse);
        staff.StaffRegistrationNumber.Should().Be("SANC-2024-9871");
        staff.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithMissingClinicId_ThrowsDomainException()
    {
        // Act
        var act = () => ClinicStaff.Create(
            Guid.Empty,
            "Nomvula",
            "Mthembu",
            "nomvula@kznhealth.gov.za",
            "+27 35 572 1042",
            StaffRole.Nurse,
            "SANC-2024-9871");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Clinic identifier is required.");
    }

    [Fact]
    public void Update_ModifiesStaffDetails()
    {
        // Arrange
        var staff = ClinicStaff.Create(
            Guid.NewGuid(),
            "Nomvula",
            "Mthembu",
            "nomvula@kznhealth.gov.za",
            "+27 35 572 1042",
            StaffRole.Nurse,
            "SANC-2024-9871");

        // Act
        staff.Update(
            "Nomvula",
            "Khumalo",
            "+27 35 572 9999",
            StaffRole.Manager,
            "SANC-2024-9871");

        // Assert
        staff.LastName.Should().Be("Khumalo");
        staff.FullName.Should().Be("Nomvula Khumalo");
        staff.PhoneNumber.Should().Be("+27 35 572 9999");
        staff.Role.Should().Be(StaffRole.Manager);
    }
}
