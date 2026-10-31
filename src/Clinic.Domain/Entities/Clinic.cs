using Clinic.Domain.Common;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents a medical clinic registered within the PHCIS Clinic Booking System.
/// </summary>
public class Clinic : BaseEntity
{
    private readonly List<TimeSlot>    _timeSlots    = new();
    private readonly List<Appointment> _appointments = new();

    // -------------------------------------------------------------------------
    // Private constructor — use the factory method to create instances.
    // -------------------------------------------------------------------------
    private Clinic() { }

    // -------------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------------

    /// <summary>Gets the display name of the clinic.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the physical address of the clinic.</summary>
    public string Address { get; private set; } = string.Empty;

    /// <summary>Gets the clinic's contact phone number.</summary>
    public string Phone { get; private set; } = string.Empty;

    /// <summary>Gets the clinic's contact email address.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Gets a value indicating whether the clinic is currently active and accepting bookings.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the time of day at which the clinic opens (e.g. <c>08:00:00</c>).</summary>
    public TimeSpan OpeningTime { get; private set; }

    /// <summary>Gets the time of day at which the clinic closes (e.g. <c>17:00:00</c>).</summary>
    public TimeSpan ClosingTime { get; private set; }

    /// <summary>Gets the duration, in minutes, of each bookable appointment slot.</summary>
    public int SlotDurationMinutes { get; private set; }

    /// <summary>Gets the read-only collection of time slots defined for this clinic.</summary>
    public IReadOnlyCollection<TimeSlot> TimeSlots => _timeSlots.AsReadOnly();

    /// <summary>Gets the read-only collection of appointments associated with this clinic.</summary>
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates and returns a new, active <see cref="Clinic"/> instance after
    /// validating all required fields.
    /// </summary>
    /// <param name="name">The display name of the clinic.</param>
    /// <param name="address">The physical address of the clinic.</param>
    /// <param name="phone">The clinic's contact phone number.</param>
    /// <param name="email">The clinic's contact email address.</param>
    /// <param name="openingTime">The time of day the clinic opens.</param>
    /// <param name="closingTime">The time of day the clinic closes.</param>
    /// <param name="slotDurationMinutes">Duration in minutes for each appointment slot.</param>
    /// <returns>A newly created <see cref="Clinic"/>.</returns>
    /// <exception cref="DomainException">
    /// Thrown when any required field is null/whitespace or when the time/duration
    /// arguments violate business rules.
    /// </exception>
    public static Clinic Create(
        string name,
        string address,
        string phone,
        string email,
        TimeSpan openingTime,
        TimeSpan closingTime,
        int slotDurationMinutes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Clinic name is required.");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Clinic address is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Clinic phone number is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Clinic email address is required.");

        if (closingTime <= openingTime)
            throw new DomainException("Clinic closing time must be later than the opening time.");

        if (slotDurationMinutes <= 0)
            throw new DomainException("Slot duration must be a positive number of minutes.");

        var now = DateTime.UtcNow;

        return new Clinic
        {
            Id                  = Guid.NewGuid(),
            Name                = name.Trim(),
            Address             = address.Trim(),
            Phone               = phone.Trim(),
            Email               = email.Trim(),
            OpeningTime         = openingTime,
            ClosingTime         = closingTime,
            SlotDurationMinutes = slotDurationMinutes,
            IsActive            = true,
            CreatedAt           = now,
            UpdatedAt           = now
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Updates the clinic's mutable details.
    /// </summary>
    /// <param name="name">New display name.</param>
    /// <param name="address">New physical address.</param>
    /// <param name="phone">New contact phone number.</param>
    /// <param name="email">New contact email address.</param>
    /// <param name="openingTime">New opening time.</param>
    /// <param name="closingTime">New closing time.</param>
    /// <param name="slotDurationMinutes">New slot duration in minutes.</param>
    /// <exception cref="DomainException">Thrown when any argument violates validation rules.</exception>
    public void Update(
        string name,
        string address,
        string phone,
        string email,
        TimeSpan openingTime,
        TimeSpan closingTime,
        int slotDurationMinutes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Clinic name is required.");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Clinic address is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Clinic phone number is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Clinic email address is required.");

        if (closingTime <= openingTime)
            throw new DomainException("Clinic closing time must be later than the opening time.");

        if (slotDurationMinutes <= 0)
            throw new DomainException("Slot duration must be a positive number of minutes.");

        Name                = name.Trim();
        Address             = address.Trim();
        Phone               = phone.Trim();
        Email               = email.Trim();
        OpeningTime         = openingTime;
        ClosingTime         = closingTime;
        SlotDurationMinutes = slotDurationMinutes;
        UpdatedAt           = DateTime.UtcNow;
    }

    /// <summary>Marks the clinic as active so it can accept new bookings.</summary>
    public void Activate()
    {
        IsActive  = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Marks the clinic as inactive, preventing new bookings.</summary>
    public void Deactivate()
    {
        IsActive  = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
