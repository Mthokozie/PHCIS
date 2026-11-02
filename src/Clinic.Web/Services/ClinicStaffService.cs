using System.Net.Http.Json;
using Clinic.Web.Models.Staff;

namespace Clinic.Web.Services;

public class ClinicStaffService : IClinicStaffService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public ClinicStaffService(HttpClient httpClient, IAuthService authService)
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

    public async Task<ClinicStaffDto?> GetStaffByIdAsync(Guid staffId)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<ClinicStaffDto>($"api/clinicstaff/{staffId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<ClinicStaffDto>> GetStaffByClinicAsync(Guid clinicId)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<List<ClinicStaffDto>>($"api/clinicstaff/clinic/{clinicId}")
                   ?? new List<ClinicStaffDto>();
        }
        catch
        {
            return new List<ClinicStaffDto>();
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> RegisterStaffAsync(RegisterStaffRequest request)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PostAsJsonAsync("api/auth/register-staff", request);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            var err = await response.Content.ReadAsStringAsync();
            return (false, $"Registration failed: {response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}");
        }
    }
}
