namespace Clinic.Web.Models.Staff;

public class RegisterStaffRequest
{
    public Guid ClinicId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Role { get; set; }
    public string StaffRegistrationNumber { get; set; } = string.Empty;
}
