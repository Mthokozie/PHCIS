using System.Net.Http.Json;
using Clinic.Web.Models;
using Clinic.Web.Models.Patients;

namespace Clinic.Web.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public PatientService(HttpClient httpClient, IAuthService authService)
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

    public async Task<PatientDto?> GetPatientByIdAsync(Guid id)
    {
        try
        {
            await AttachTokenAsync();
            return await _httpClient.GetFromJsonAsync<PatientDto>($"api/patients/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdatePatientAsync(UpdatePatientRequest request)
    {
        try
        {
            await AttachTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/patients/{request.Id}", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
