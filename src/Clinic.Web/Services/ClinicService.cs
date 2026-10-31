using System.Net.Http.Json;
using Clinic.Web.Models;

namespace Clinic.Web.Services;

public class ClinicService : IClinicService
{
    private readonly HttpClient _httpClient;

    public ClinicService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClinicDto>> GetAllClinicsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ClinicDto>>("api/clinics")
                   ?? new List<ClinicDto>();
        }
        catch
        {
            return new List<ClinicDto>();
        }
    }

    public async Task<ClinicDto?> GetClinicByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ClinicDto>($"api/clinics/{id}");
        }
        catch
        {
            return null;
        }
    }
}
