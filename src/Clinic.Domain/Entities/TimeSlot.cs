using Clinic.Domain.Common;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents a bookable time slot within a clinic's schedule.
/// A time slot has a finite capacity and tracks how many appointments
/// have been reserved against it.
/// </summary>
public class TimeSlot : BaseEntity
{
    private readonly List<Appointment> _appointments = new();

    // -------------------------------------------------------------------------
    // Private constructor — use the factory method to create instances.
    // -------------------------------------------------------------------------
    private TimeSlot() { }

    // -------------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------------

    /// <summary>Gets the identifier of the clinic that owns this time slot.</summary>
    public Guid ClinicId { get; private set; }

    /// <summary>Gets the clinic that owns this time slot (navigation property).</summary>
    public Clinic Clinic { get; private set; } = null!;

    /// <summary>Gets the UTC date and time at which this slot begins.</summary>
    public DateTime StartTime { get; private set; }

    /// <summary>Gets the UTC date and time at which this slot ends.</summary>
    public DateTime EndTime { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this slot is currently accepting new bookings.
    /// Set to <see langword="false"/> automatically when <see cref="CurrentBookings"/>
    /// reaches <see cref="MaxCapacity"/>.
    /// </summary>
    public bool IsAvailable { get; private set; }

    /// <summary>Gets the maximum number of concurrent appointments allowed in this slot.</summary>
    public int MaxCapacity { get; private set; }

    /// <summary>Gets the number of appointments currently booked against this slot.</summary>
    public int CurrentBookings { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there is at least one free place remaining in this slot.
    /// </summary>
    public bool HasAvailableCapacity => CurrentBookings < MaxCapacity;

    /// <summary>
    /// Gets the number of places still available in this slot.
    /// </summary>
    public int RemainingCapacity => MaxCapacity - CurrentBookings;

    /// <summary>Gets the read-only collection of appointments booked for this time slot.</summary>
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates and returns a new <see cref="TimeSlot"/> instance after validating
    /// all required arguments.
    /// </summary>
    /// <param name="clinicId">The identifier of the owning clinic.</param>
    /// <param name="startTime">The UTC start date/time of the slot.</param>
    /// <param name="endTime">The UTC end date/time of the slot.</param>
    /// <param name="maxCapacity">The maximum number of bookings allowed.</param>
    /// <returns>A newly created <see cref="TimeSlot"/>.</returns>
    /// <exception cref="DomainException">
    /// Thrown when <paramref name="endTime"/> is not later than <paramref name="startTime"/>,
    /// when <paramref name="clinicId"/> is empty, or when <paramref name="maxCapacity"/>
    /// is not positive.
    /// </exception>
    public static TimeSlot Create(
        Guid clinicId,
        DateTime startTime,
        DateTime endTime,
        int maxCapacity)
    {
        if (clinicId == Guid.Empty)
            throw new DomainException("A valid clinic identifier is required to create a time slot.");

        if (endTime <= startTime)
            throw new DomainException("Time slot end time must be later than the start time.");

        if (maxCapacity <= 0)
            throw new DomainException("Time slot maximum capacity must be a positive integer.");

        var now = DateTime.UtcNow;

        return new TimeSlot
        {
            Id              = Guid.NewGuid(),
            ClinicId        = clinicId,
            StartTime       = startTime,
            EndTime         = endTime,
            MaxCapacity     = maxCapacity,
            CurrentBookings = 0,
            IsAvailable     = true,
            CreatedAt       = now,
            UpdatedAt       = now
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Reserves one place in this time slot, incrementing <see cref="CurrentBookings"/>.
    /// Sets <see cref="IsAvailable"/> to <see langword="false"/> when the slot becomes full.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when <see cref="HasAvailableCapacity"/> is <see langword="false"/>.
    /// </exception>
    public void Reserve()
    {
        if (!HasAvailableCapacity)
            throw new DomainException(
                $"Time slot '{Id}' has no remaining capacity (max: {MaxCapacity}).");

        CurrentBookings++;

        if (!HasAvailableCapacity)
            IsAvailable = false;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Releases one previously reserved place in this time slot, decrementing
    /// <see cref="CurrentBookings"/> and re-enabling <see cref="IsAvailable"/> if required.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown when <see cref="CurrentBookings"/> is already zero.
    /// </exception>
    public void Release()
    {
        if (CurrentBookings <= 0)
            throw new DomainException(
                $"Time slot '{Id}' has no bookings to release.");

        CurrentBookings--;
        IsAvailable = true;
        UpdatedAt   = DateTime.UtcNow;
    }
}
