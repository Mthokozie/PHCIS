namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object representing an appointment.
/// </summary>
public class AppointmentDto
{
    /// <summary>Gets or sets the appointment identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the patient identifier.</summary>
    public Guid PatientId { get; set; }

    /// <summary>Gets or sets the patient full name.</summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic identifier.</summary>
    public Guid ClinicId { get; set; }

    /// <summary>Gets or sets the clinic name.</summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>Gets or sets the time slot identifier.</summary>
    public Guid TimeSlotId { get; set; }

    /// <summary>Gets or sets the slot start time.</summary>
    public DateTime SlotStartTime { get; set; }

    /// <summary>Gets or sets the slot end time.</summary>
    public DateTime SlotEndTime { get; set; }

    /// <summary>Gets or sets the attending staff member identifier, if assigned.</summary>
    public Guid? AttendingStaffId { get; set; }

    /// <summary>Gets or sets the attending staff member's full name, if assigned.</summary>
    public string? AttendingStaffName { get; set; }

    /// <summary>Gets or sets the appointment status.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Gets or sets the unique confirmation code.</summary>
    public string ConfirmationCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the reason for the appointment.</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>Gets or sets the date and time the appointment was booked.</summary>
    public DateTime BookedAt { get; set; }

    /// <summary>Gets or sets the timestamp when consultation was started.</summary>
    public DateTime? ConsultationStartedAt { get; set; }

    /// <summary>Gets or sets the timestamp when consultation was completed or referred.</summary>
    public DateTime? ConsultationCompletedAt { get; set; }

    /// <summary>Gets or sets the consultation summary / notes recorded by staff.</summary>
    public string? ConsultationNotes { get; set; }

    /// <summary>Gets or sets the destination hospital name if referred.</summary>
    public string? ReferralHospital { get; set; }

    /// <summary>Gets or sets the clinical referral reason if referred.</summary>
    public string? ReferralReason { get; set; }

    /// <summary>Gets or sets the cancellation reason if cancelled.</summary>
    public string? CancellationReason { get; set; }
}
