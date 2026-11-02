namespace Clinic.Web.Models.Appointments;

public class AcceptAppointmentRequest
{
    public Guid StaffId { get; set; }
}

public class ProcessAppointmentRequest
{
    public Guid StaffId { get; set; }
}

public class CompleteConsultationRequest
{
    public Guid StaffId { get; set; }
    public string? Notes { get; set; }
}

public class ReferHospitalRequest
{
    public Guid StaffId { get; set; }
    public string HospitalName { get; set; } = string.Empty;
    public string ReferralReason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateConsultationNotesRequest
{
    public Guid StaffId { get; set; }
    public string Notes { get; set; } = string.Empty;
}
