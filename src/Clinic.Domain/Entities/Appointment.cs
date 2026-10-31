using System.Text;
using Clinic.Domain.Common;
using Clinic.Domain.Enums;
using Clinic.Domain.Events;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents a patient's booking for a specific clinic time slot.
/// Encapsulates the full appointment lifecycle from pending through to
/// completed, cancelled, or no-show.
/// </summary>
public class Appointment : BaseEntity
{
    private const string ConfirmationCodePrefix = "PHCIS-";
    private const int ConfirmationCodeRandomLength = 8;
    private const string ConfirmationCodeChars =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    // -------------------------------------------------------------------------
    // Private constructor — use the factory method to create instances.
    // -------------------------------------------------------------------------
    private Appointment() { }

    // -------------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------------

    /// <summary>Gets the identifier of the patient who booked this appointment.</summary>
    public Guid PatientId { get; private set; }

    /// <summary>Gets the patient who booked this appointment (navigation property).</summary>
    public Patient Patient { get; private set; } = null!;

    /// <summary>Gets the identifier of the clinic at which the appointment is booked.</summary>
    public Guid ClinicId { get; private set; }

    /// <summary>Gets the clinic at which the appointment is booked (navigation property).</summary>
    public Clinic Clinic { get; private set; } = null!;

    /// <summary>Gets the identifier of the time slot reserved by this appointment.</summary>
    public Guid TimeSlotId { get; private set; }

    /// <summary>Gets the time slot reserved by this appointment (navigation property).</summary>
    public TimeSlot TimeSlot { get; private set; } = null!;

    /// <summary>Gets the current status of the appointment.</summary>
    public AppointmentStatus Status { get; private set; }

    /// <summary>Gets the patient-provided reason for the appointment.</summary>
    public string Reason { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the unique, human-readable confirmation code for this appointment
    /// (format: <c>PHCIS-XXXXXXXX</c>).
    /// </summary>
    public string ConfirmationCode { get; private set; } = string.Empty;

    /// <summary>Gets the UTC date and time at which this appointment was booked.</summary>
    public DateTime BookedAt { get; private set; }

    /// <summary>
    /// Gets the UTC date and time at which this appointment was cancelled,
    /// or <see langword="null"/> if it has not been cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// Gets the reason provided when cancelling this appointment,
    /// or <see langword="null"/> if it has not been cancelled.
    /// </summary>
    public string? CancellationReason { get; private set; }

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates and returns a new <see cref="Appointment"/> in <see cref="AppointmentStatus.Pending"/>
    /// status and raises an <see cref="AppointmentBookedEvent"/>.
    /// </summary>
    /// <param name="patientId">The identifier of the booking patient.</param>
    /// <param name="clinicId">The identifier of the target clinic.</param>
    /// <param name="timeSlotId">The identifier of the time slot being reserved.</param>
    /// <param name="reason">The patient's stated reason for the appointment.</param>
    /// <returns>A newly created <see cref="Appointment"/>.</returns>
    /// <exception cref="DomainException">
    /// Thrown when any identifier is empty or <paramref name="reason"/> is null/whitespace.
    /// </exception>
    public static Appointment Create(
        Guid patientId,
        Guid clinicId,
        Guid timeSlotId,
        string reason)
    {
        if (patientId == Guid.Empty)
            throw new DomainException("A valid patient identifier is required.");

        if (clinicId == Guid.Empty)
            throw new DomainException("A valid clinic identifier is required.");

        if (timeSlotId == Guid.Empty)
            throw new DomainException("A valid time slot identifier is required.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("A reason for the appointment is required.");

        var now = DateTime.UtcNow;
        var confirmationCode = GenerateConfirmationCode();

        var appointment = new Appointment
        {
            Id               = Guid.NewGuid(),
            PatientId        = patientId,
            ClinicId         = clinicId,
            TimeSlotId       = timeSlotId,
            Reason           = reason.Trim(),
            Status           = AppointmentStatus.Pending,
            ConfirmationCode = confirmationCode,
            BookedAt         = now,
            CreatedAt        = now,
            UpdatedAt        = now
        };

        appointment.AddDomainEvent(new AppointmentBookedEvent(
            appointment.Id,
            patientId,
            clinicId,
            confirmationCode));

        return appointment;
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Confirms the appointment, transitioning its status to
    /// <see cref="AppointmentStatus.Confirmed"/>.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when the appointment is not in <see cref="AppointmentStatus.Pending"/> status.
    /// </exception>
    public void Confirm()
    {
        if (Status != AppointmentStatus.Pending)
            throw new DomainException(
                $"Only pending appointments can be confirmed. Current status: {Status}.");

        Status    = AppointmentStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the appointment, transitioning its status to
    /// <see cref="AppointmentStatus.Cancelled"/> and raises an
    /// <see cref="AppointmentCancelledEvent"/>.
    /// </summary>
    /// <param name="reason">The reason for cancellation.</param>
    /// <exception cref="DomainException">
    /// Thrown when the appointment is already cancelled or completed.
    /// </exception>
    public void Cancel(string reason)
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new DomainException("The appointment has already been cancelled.");

        if (Status == AppointmentStatus.Completed)
            throw new DomainException("A completed appointment cannot be cancelled.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("A cancellation reason is required.");

        var now = DateTime.UtcNow;

        Status             = AppointmentStatus.Cancelled;
        CancellationReason = reason.Trim();
        CancelledAt        = now;
        UpdatedAt          = now;

        AddDomainEvent(new AppointmentCancelledEvent(Id, reason.Trim()));
    }

    /// <summary>
    /// Marks the appointment as completed, transitioning its status to
    /// <see cref="AppointmentStatus.Completed"/>.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when the appointment is not in <see cref="AppointmentStatus.Confirmed"/> status.
    /// </exception>
    public void Complete()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new DomainException(
                $"Only confirmed appointments can be marked as completed. Current status: {Status}.");

        Status    = AppointmentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the appointment as a no-show, transitioning its status to
    /// <see cref="AppointmentStatus.NoShow"/>.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when the appointment is not in <see cref="AppointmentStatus.Confirmed"/> status.
    /// </exception>
    public void MarkAsNoShow()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new DomainException(
                $"Only confirmed appointments can be marked as no-show. Current status: {Status}.");

        Status    = AppointmentStatus.NoShow;
        UpdatedAt = DateTime.UtcNow;
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Generates a unique confirmation code in the format <c>PHCIS-XXXXXXXX</c>
    /// where <c>X</c> is a random uppercase letter or digit.
    /// </summary>
    private static string GenerateConfirmationCode()
    {
        var random = Random.Shared;
        var sb = new StringBuilder(ConfirmationCodePrefix.Length + ConfirmationCodeRandomLength);
        sb.Append(ConfirmationCodePrefix);

        for (var i = 0; i < ConfirmationCodeRandomLength; i++)
            sb.Append(ConfirmationCodeChars[random.Next(ConfirmationCodeChars.Length)]);

        return sb.ToString();
    }
}
