namespace Clinic.Web.Models;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid ClinicId { get; set; }
    public string ClinicName { get; set; } = string.Empty;
    public DateTime SlotStartTime { get; set; }
    public DateTime SlotEndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ConfirmationCode { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime BookedAt { get; set; }
}
