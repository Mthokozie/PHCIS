using Clinic.Domain.Common;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;

namespace Clinic.Domain.Entities;

/// <summary>
/// Represents an internal clinic staff member (e.g., Nurse, Clerk, Doctor) assigned to a clinic.
/// Staff members process, accept, complete consultations, and issue hospital referrals.
/// </summary>
public class ClinicStaff : BaseEntity
{
    private readonly List<Appointment> _processedAppointments = new();

    private ClinicStaff() { }

    /// <summary>Gets the identifier of the clinic this staff member belongs to.</summary>
    public Guid ClinicId { get; private set; }

    /// <summary>Gets the clinic navigation property.</summary>
    public Clinic Clinic { get; private set; } = null!;

    /// <summary>Gets the staff member's first name.</summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>Gets the staff member's last name / surname.</summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>Gets the staff member's official professional email address.</summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>Gets the contact phone number.</summary>
    public string PhoneNumber { get; private set; } = string.Empty;

    /// <summary>Gets the staff member's professional role within the clinic.</summary>
    public StaffRole Role { get; private set; }

    /// <summary>Gets the professional registration or employee license number (e.g. SANC, HPCSA, or Staff ID).</summary>
    public string StaffRegistrationNumber { get; private set; } = string.Empty;

    /// <summary>Gets a value indicating whether this staff member is actively working at the clinic.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the full computed display name of the staff member.</summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Gets appointments processed, attended, or accepted by this staff member.</summary>
    public IReadOnlyCollection<Appointment> ProcessedAppointments => _processedAppointments.AsReadOnly();

    /// <summary>
    /// Factory method to create a new validated <see cref="ClinicStaff"/> member.
    /// </summary>
    public static ClinicStaff Create(
        Guid clinicId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        StaffRole role,
        string staffRegistrationNumber)
    {
        if (clinicId == Guid.Empty)
            throw new DomainException("Clinic identifier is required.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(staffRegistrationNumber))
            throw new DomainException("Staff registration number is required.");

        var now = DateTime.UtcNow;

        return new ClinicStaff
        {
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PhoneNumber = phoneNumber.Trim(),
            Role = role,
            StaffRegistrationNumber = staffRegistrationNumber.Trim().ToUpperInvariant(),
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    /// <summary>
    /// Updates staff member personal details and professional role.
    /// </summary>
    public void Update(
        string firstName,
        string lastName,
        string phoneNumber,
        StaffRole role,
        string staffRegistrationNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("Phone number is required.");

        if (string.IsNullOrWhiteSpace(staffRegistrationNumber))
            throw new DomainException("Staff registration number is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber.Trim();
        Role = role;
        StaffRegistrationNumber = staffRegistrationNumber.Trim().ToUpperInvariant();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Deactivates the staff member account.</summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Activates or reinstates the staff member account.</summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
