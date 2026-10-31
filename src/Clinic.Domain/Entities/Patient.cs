using Clinic.Domain.Common;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents a patient registered in the PHCIS Clinic Booking System.
/// </summary>
public class Patient : BaseEntity
{
    private readonly List<Appointment> _appointments = new();

    // -------------------------------------------------------------------------
    // Private constructor — use the factory method to create instances.
    // -------------------------------------------------------------------------
    private Patient() { }

    // -------------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------------

    /// <summary>Gets the patient's first name.</summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>Gets the patient's last name / surname.</summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>Gets the patient's email address.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Gets the patient's contact phone number.</summary>
    public string PhoneNumber { get; private set; } = string.Empty;

    /// <summary>Gets the patient's date of birth.</summary>
    public DateTime DateOfBirth { get; private set; }

    /// <summary>Gets the patient's national / government-issued identity number.</summary>
    public string IdNumber { get; private set; } = string.Empty;

    /// <summary>Gets a value indicating whether the patient account is currently active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the patient's full name, computed as <c>FirstName LastName</c>.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the read-only collection of appointments associated with this patient.
    /// </summary>
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    // -------------------------------------------------------------------------
    // Factory
    // -------------------------------------------------------------------------

    /// <summary>
    /// Creates and returns a new, active <see cref="Patient"/> instance after
    /// validating all required fields.
    /// </summary>
    /// <param name="firstName">The patient's first name.</param>
    /// <param name="lastName">The patient's last name.</param>
    /// <param name="email">The patient's email address.</param>
    /// <param name="phone">The patient's phone number.</param>
    /// <param name="dob">The patient's date of birth.</param>
    /// <param name="idNumber">The patient's national identity number.</param>
    /// <returns>A newly created <see cref="Patient"/>.</returns>
    /// <exception cref="DomainException">
    /// Thrown when any required field is null or whitespace.
    /// </exception>
    public static Patient Create(
        string firstName,
        string lastName,
        string email,
        string phone,
        DateTime dob,
        string idNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Patient first name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Patient last name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Patient email address is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Patient phone number is required.");

        if (string.IsNullOrWhiteSpace(idNumber))
            throw new DomainException("Patient identity number is required.");

        if (dob >= DateTime.UtcNow)
            throw new DomainException("Patient date of birth must be in the past.");

        var now = DateTime.UtcNow;

        return new Patient
        {
            Id            = Guid.NewGuid(),
            FirstName     = firstName.Trim(),
            LastName      = lastName.Trim(),
            Email         = email.Trim(),
            PhoneNumber   = phone.Trim(),
            DateOfBirth   = dob,
            IdNumber      = idNumber.Trim(),
            IsActive      = true,
            CreatedAt     = now,
            UpdatedAt     = now
        };
    }

    // -------------------------------------------------------------------------
    // Behaviour
    // -------------------------------------------------------------------------

    /// <summary>
    /// Updates the patient's mutable personal details.
    /// </summary>
    /// <param name="firstName">New first name.</param>
    /// <param name="lastName">New last name.</param>
    /// <param name="phone">New phone number.</param>
    /// <exception cref="DomainException">Thrown when any argument is null or whitespace.</exception>
    public void Update(string firstName, string lastName, string phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Patient first name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Patient last name is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Patient phone number is required.");

        FirstName   = firstName.Trim();
        LastName    = lastName.Trim();
        PhoneNumber = phone.Trim();
        UpdatedAt   = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the patient as inactive, preventing future bookings.
    /// </summary>
    public void Deactivate()
    {
        IsActive  = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
