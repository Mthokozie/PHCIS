namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object representing an internal clinic staff member.
/// </summary>
public class ClinicStaffDto
{
    /// <summary>Gets or sets the staff member's unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the clinic identifier.</summary>
    public Guid ClinicId { get; set; }

    /// <summary>Gets or sets the clinic name.</summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>Gets or sets the staff member's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the staff member's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the staff member's computed full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the staff member's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the contact phone number.</summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets the staff member's professional role.</summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>Gets or sets the professional registration number (e.g. SANC, HPCSA, Staff ID).</summary>
    public string StaffRegistrationNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets a value indicating whether the staff member is actively working.</summary>
    public bool IsActive { get; set; }
}
