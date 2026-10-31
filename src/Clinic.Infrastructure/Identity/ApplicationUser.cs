namespace Clinic.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Extended Identity user with optional link to a Patient record.
/// Supports roles: Patient, ClinicStaff, Admin.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Gets or sets the role of this user (Patient, ClinicStaff, Admin).</summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>Gets or sets the linked patient ID (null if not a patient user).</summary>
    public Guid? PatientId { get; set; }
}
