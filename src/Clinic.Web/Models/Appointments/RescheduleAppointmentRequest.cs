namespace Clinic.Web.Models.Appointments;

public class RescheduleAppointmentRequest
{
    public Guid NewTimeSlotId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
