namespace Clinic.Domain.Enums;

/// <summary>
/// Represents the role of a user within the PHCIS Clinic Booking System.
/// </summary>
public enum UserRole
{
    /// <summary>A registered patient who can book appointments.</summary>
    Patient = 0,

    /// <summary>A clinic staff member who manages appointments and time slots.</summary>
    ClinicStaff = 1,

    /// <summary>A system administrator with full access.</summary>
    Admin = 2
}
