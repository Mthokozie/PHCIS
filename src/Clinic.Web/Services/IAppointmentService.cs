using Clinic.Web.Models;
using Clinic.Web.Models.Appointments;

namespace Clinic.Web.Services;

public interface IAppointmentService
{
    Task<AppointmentConfirmationDto?> BookAppointmentAsync(BookAppointmentRequest request);
    Task<bool> CancelAppointmentAsync(Guid appointmentId, string reason);
    Task<List<AppointmentDto>> GetPatientAppointmentsAsync(Guid patientId);
    Task<AppointmentDto?> GetAppointmentByIdAsync(Guid id);
}
