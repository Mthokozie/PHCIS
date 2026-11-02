using Clinic.Web.Models;
using Clinic.Web.Models.Appointments;

namespace Clinic.Web.Services;

public interface IAppointmentService
{
    Task<AppointmentConfirmationDto?> BookAppointmentAsync(BookAppointmentRequest request);
    Task<bool> CancelAppointmentAsync(Guid appointmentId, string reason);
    Task<List<AppointmentDto>> GetPatientAppointmentsAsync(Guid patientId);
    Task<List<AppointmentDto>> GetClinicAppointmentsAsync(Guid clinicId);
    Task<AppointmentDto?> GetAppointmentByIdAsync(Guid id);
    Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> AcceptAppointmentAsync(Guid appointmentId, Guid staffId);
    Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> ProcessAppointmentAsync(Guid appointmentId, Guid staffId);
    Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> CompleteConsultationAsync(Guid appointmentId, Guid staffId, string? notes);
    Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> ReferAppointmentAsync(Guid appointmentId, Guid staffId, string hospitalName, string referralReason, string? notes);
    Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> UpdateConsultationNotesAsync(Guid appointmentId, Guid staffId, string notes);
}
