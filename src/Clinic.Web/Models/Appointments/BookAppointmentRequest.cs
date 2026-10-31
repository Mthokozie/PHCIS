namespace Clinic.Web.Models.Appointments;

public class BookAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid TimeSlotId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
