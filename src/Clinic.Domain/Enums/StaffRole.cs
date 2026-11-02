namespace Clinic.Domain.Enums;

/// <summary>
/// Roles assigned to clinic staff members.
/// </summary>
public enum StaffRole
{
    /// <summary>Clinical nurse providing consultations and primary care.</summary>
    Nurse = 0,

    /// <summary>Administrative clerk managing bookings, reception, and patient queue.</summary>
    Clerk = 1,

    /// <summary>Medical doctor or clinical officer.</summary>
    Doctor = 2,

    /// <summary>Clinic manager or operational supervisor.</summary>
    Manager = 3
}
