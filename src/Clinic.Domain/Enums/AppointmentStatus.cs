namespace Clinic.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a clinic appointment.
/// </summary>
public enum AppointmentStatus
{
    /// <summary>The appointment has been booked but not yet confirmed by the clinic.</summary>
    Pending = 0,

    /// <summary>The appointment has been confirmed by the clinic.</summary>
    Confirmed = 1,

    /// <summary>The appointment was cancelled by the patient or clinic staff.</summary>
    Cancelled = 2,

    /// <summary>The appointment was attended and the consultation is complete.</summary>
    Completed = 3,

    /// <summary>The patient did not attend the appointment.</summary>
    NoShow = 4,

    /// <summary>The patient is currently in consultation / being processed by clinic staff.</summary>
    InConsultation = 5,

    /// <summary>The consultation resulted in a clinical referral to a district/regional hospital.</summary>
    Referred = 6
}
