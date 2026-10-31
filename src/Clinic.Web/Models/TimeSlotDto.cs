namespace Clinic.Web.Models;

public class TimeSlotDto
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public string ClinicName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public int RemainingCapacity { get; set; }
}
