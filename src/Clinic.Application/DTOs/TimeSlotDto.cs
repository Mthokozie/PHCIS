namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a bookable time slot.
/// </summary>
public class TimeSlotDto
{
    /// <summary>Gets or sets the time slot identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the clinic identifier.</summary>
    public Guid ClinicId { get; set; }

    /// <summary>Gets or sets the clinic name.</summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>Gets or sets the start time of the slot.</summary>
    public DateTime StartTime { get; set; }

    /// <summary>Gets or sets the end time of the slot.</summary>
    public DateTime EndTime { get; set; }

    /// <summary>Gets or sets a value indicating whether the slot is available.</summary>
    public bool IsAvailable { get; set; }

    /// <summary>Gets or sets the remaining capacity of the slot.</summary>
    public int RemainingCapacity { get; set; }
}
