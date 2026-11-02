namespace Clinic.Domain.Tests.Entities;

using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;
using FluentAssertions;
using Xunit;

public class AppointmentStaffProcessingTests
{
    private static Appointment CreateValidAppointment()
    {
        return Appointment.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Routine primary health consultation");
    }

    [Fact]
    public void AcceptByStaff_SetsStatusConfirmedAndAttendingStaff()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();

        // Act
        appointment.AcceptByStaff(staffId);

        // Assert
        appointment.AttendingStaffId.Should().Be(staffId);
        appointment.Status.Should().Be(AppointmentStatus.Confirmed);
    }

    [Fact]
    public void StartProcessing_SetsStatusInConsultationAndTimestamp()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.AcceptByStaff(staffId);

        // Act
        appointment.StartProcessing(staffId);

        // Assert
        appointment.AttendingStaffId.Should().Be(staffId);
        appointment.Status.Should().Be(AppointmentStatus.InConsultation);
        appointment.ConsultationStartedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsCompleted_SetsStatusCompletedAndNotes()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.StartProcessing(staffId);

        // Act
        appointment.MarkAsCompleted(staffId, "Patient vitals normal, prescribed routine medication.");

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.Completed);
        appointment.ConsultationNotes.Should().Be("Patient vitals normal, prescribed routine medication.");
        appointment.ConsultationCompletedAt.Should().NotBeNull();
    }

    [Fact]
    public void ReferToHospital_SetsStatusReferredAndHospitalDetails()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.StartProcessing(staffId);

        // Act
        appointment.ReferToHospital(
            staffId,
            "Mosvold District Hospital",
            "Acute surgical evaluation required",
            "Urgent referral transfer letter issued");

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.Referred);
        appointment.ReferralHospital.Should().Be("Mosvold District Hospital");
        appointment.ReferralReason.Should().Be("Acute surgical evaluation required");
        appointment.ConsultationNotes.Should().Be("Urgent referral transfer letter issued");
        appointment.ConsultationCompletedAt.Should().NotBeNull();
    }

    [Fact]
    public void ReferToHospital_WithoutHospitalName_ThrowsDomainException()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.StartProcessing(staffId);

        // Act
        var act = () => appointment.ReferToHospital(staffId, "", "Severe hypertension");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Referral hospital name is required.");
    }

    [Fact]
    public void UpdateConsultationNotes_WhileInConsultation_SavesNotesSuccessfully()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.StartProcessing(staffId);

        // Act
        appointment.UpdateConsultationNotes(staffId, "Patient presents with persistent cough for 3 days. Vitals: BP 120/80, Temp 37.1 C.");

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.InConsultation);
        appointment.ConsultationNotes.Should().Be("Patient presents with persistent cough for 3 days. Vitals: BP 120/80, Temp 37.1 C.");
        appointment.AttendingStaffId.Should().Be(staffId);
    }

    [Fact]
    public void UpdateConsultationNotes_WhenCancelled_ThrowsDomainException()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var staffId = Guid.NewGuid();
        appointment.Cancel("Patient did not arrive");

        // Act
        var act = () => appointment.UpdateConsultationNotes(staffId, "Test note");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Consultation notes can only be recorded while an appointment is in consultation or confirmed*");
    }
}
