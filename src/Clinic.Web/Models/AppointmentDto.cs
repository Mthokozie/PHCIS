namespace Clinic.Web.Models;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid ClinicId { get; set; }
    public string ClinicName { get; set; } = string.Empty;
    public Guid TimeSlotId { get; set; }
    public DateTime SlotStartTime { get; set; }
    public DateTime SlotEndTime { get; set; }
    public Guid? AttendingStaffId { get; set; }
    public string? AttendingStaffName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ConfirmationCode { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime BookedAt { get; set; }
    public DateTime? ConsultationStartedAt { get; set; }
    public DateTime? ConsultationCompletedAt { get; set; }
    public string? ConsultationNotes { get; set; }
    public string? ReferralHospital { get; set; }
    public string? ReferralReason { get; set; }
    public string? CancellationReason { get; set; }
}
