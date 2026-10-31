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

    /// <summary>Gets or sets the appointment status.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Gets or sets the unique confirmation code.</summary>
    public string ConfirmationCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the reason for the appointment.</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>Gets or sets the date and time the appointment was booked.</summary>
    public DateTime BookedAt { get; set; }
}
