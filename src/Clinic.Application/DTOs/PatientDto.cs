namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a patient.
/// </summary>
public class PatientDto
{
    /// <summary>Gets or sets the patient identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the patient's full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's phone number.</summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's identity number.</summary>
    public string IdNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets the patient's date of birth.</summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>Gets or sets a value indicating whether the patient is active.</summary>
    public bool IsActive { get; set; }
}
