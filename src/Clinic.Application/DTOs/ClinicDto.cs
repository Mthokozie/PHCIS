namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a clinic.
/// </summary>
public class ClinicDto
{
    /// <summary>Gets or sets the clinic identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the clinic display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic physical address.</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic contact phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic contact email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets a value indicating whether the clinic is active.</summary>
    public bool IsActive { get; set; }

    /// <summary>Gets or sets the clinic opening time (HH:mm).</summary>
    public string OpeningTime { get; set; } = string.Empty;

    /// <summary>Gets or sets the clinic closing time (HH:mm).</summary>
    public string ClosingTime { get; set; } = string.Empty;

    /// <summary>Gets or sets the slot duration in minutes.</summary>
    public int SlotDurationMinutes { get; set; }
}
