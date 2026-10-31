namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object returned upon successful booking or rescheduling of an appointment.
/// </summary>
public class AppointmentConfirmationDto
{
    /// <summary>Gets or sets the appointment identifier.</summary>
    public Guid AppointmentId { get; set; }

    /// <summary>Gets or sets the human-readable confirmation code (e.g., PHCIS-XXXXXXXX).</summary>
    public string ConfirmationCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's full name.</summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic name.</summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>Gets or sets the slot start time.</summary>
    public DateTime SlotStartTime { get; set; }

    /// <summary>Gets or sets the slot end time.</summary>
    public DateTime SlotEndTime { get; set; }

    /// <summary>Gets or sets the confirmation message.</summary>
    public string Message { get; set; } = string.Empty;
}
