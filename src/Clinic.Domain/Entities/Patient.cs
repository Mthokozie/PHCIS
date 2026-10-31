using Clinic.Domain.Common;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents a patient registered in the PHCIS Clinic Booking System.
/// </summary>
public class Patient : BaseEntity
{
    private readonly List<Appointment> _appointments = new();

    private Patient() { }

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

    /// <summary>Gets the patient's profile image (Data URI or URL), if provided.</summary>
    public string? ProfileImageUrl { get; private set; }

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

    /// <summary>
    /// Creates and returns a new, active <see cref="Patient"/> instance after
    /// validating all required fields.
    /// </summary>
    public static Patient Create(
        string firstName,
        string lastName,
        string email,
        string phone,
        DateTime dob,
        string idNumber,
        string? profileImageUrl = null)
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
            Id              = Guid.NewGuid(),
            FirstName       = firstName.Trim(),
            LastName        = lastName.Trim(),
            Email           = email.Trim(),
            PhoneNumber     = phone.Trim(),
            DateOfBirth     = dob,
            IdNumber        = idNumber.Trim(),
            ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? null : profileImageUrl.Trim(),
            IsActive        = true,
            CreatedAt       = now,
            UpdatedAt       = now
        };
    }

    /// <summary>
    /// Updates the patient's mutable personal details and optional profile image.
    /// </summary>
    public void Update(string firstName, string lastName, string phone, string? profileImageUrl = null)
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
        if (profileImageUrl is not null)
        {
            ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? null : profileImageUrl.Trim();
        }
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
