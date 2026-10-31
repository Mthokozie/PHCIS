using System.Net.Http.Json;
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
}
