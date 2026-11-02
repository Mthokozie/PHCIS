using System.Net.Http.Json;
using System.Text.Json;
using Clinic.Web.Models;
using Clinic.Web.Models.Appointments;

namespace Clinic.Web.Services;

public class AppointmentService : IAppointmentService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public AppointmentService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task AttachTokenAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<AppointmentConfirmationDto?> BookAppointmentAsync(BookAppointmentRequest request)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PostAsJsonAsync("api/appointments", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AppointmentConfirmationDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CancelAppointmentAsync(Guid appointmentId, string reason)
    {
        try
        {
            await AttachTokenAsync();
            var request = new CancelAppointmentRequest { Reason = reason };
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/cancel", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<AppointmentDto>> GetPatientAppointmentsAsync(Guid patientId)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>($"api/appointments/patient/{patientId}")
                   ?? new List<AppointmentDto>();
        }
        catch
        {
            return new List<AppointmentDto>();
        }
    }

    public async Task<List<AppointmentDto>> GetClinicAppointmentsAsync(Guid clinicId)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>($"api/appointments/clinic/{clinicId}")
                   ?? new List<AppointmentDto>();
        }
        catch
        {
            return new List<AppointmentDto>();
        }
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid id)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<AppointmentDto>($"api/appointments/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> AcceptAppointmentAsync(Guid appointmentId, Guid staffId)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/accept", new AcceptAppointmentRequest { StaffId = staffId });
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return (true, null, data);
            }

            var err = await ExtractErrorMessageAsync(response);
            return (false, err, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> ProcessAppointmentAsync(Guid appointmentId, Guid staffId)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/process", new ProcessAppointmentRequest { StaffId = staffId });
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return (true, null, data);
            }

            var err = await ExtractErrorMessageAsync(response);
            return (false, err, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> CompleteConsultationAsync(Guid appointmentId, Guid staffId, string? notes)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/complete", new CompleteConsultationRequest { StaffId = staffId, Notes = notes });
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return (true, null, data);
            }

            var err = await ExtractErrorMessageAsync(response);
            return (false, err, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> ReferAppointmentAsync(Guid appointmentId, Guid staffId, string hospitalName, string referralReason, string? notes)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/refer", new ReferHospitalRequest
            {
                StaffId = staffId,
                HospitalName = hospitalName,
                ReferralReason = referralReason,
                Notes = notes
            });
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return (true, null, data);
            }

            var err = await ExtractErrorMessageAsync(response);
            return (false, err, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? ErrorMessage, AppointmentDto? Data)> UpdateConsultationNotesAsync(Guid appointmentId, Guid staffId, string notes)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointmentId}/notes", new UpdateConsultationNotesRequest
            {
                StaffId = staffId,
                Notes = notes
            });
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return (true, null, data);
            }

            var err = await ExtractErrorMessageAsync(response);
            return (false, err, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("detail", out var detail))
            {
                return detail.GetString() ?? response.ReasonPhrase ?? "Request failed.";
            }
            if (doc.RootElement.TryGetProperty("message", out var msg))
            {
                return msg.GetString() ?? response.ReasonPhrase ?? "Request failed.";
            }
        }
        catch { }

        return response.ReasonPhrase ?? "Request failed.";
    }
}
