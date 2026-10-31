namespace Clinic.Web.Models;

public class AppointmentConfirmationDto
{
    public Guid AppointmentId { get; set; }
    public string ConfirmationCode { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string ClinicName { get; set; } = string.Empty;
    public DateTime SlotStartTime { get; set; }
    public DateTime SlotEndTime { get; set; }
    public string Message { get; set; } = string.Empty;
}
