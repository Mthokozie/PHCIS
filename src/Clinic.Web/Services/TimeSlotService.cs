using System.Net.Http.Json;
using Clinic.Web.Models;

namespace Clinic.Web.Services;

public class TimeSlotService : ITimeSlotService
{
    private readonly HttpClient _httpClient;

    public TimeSlotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TimeSlotDto>> GetAvailableSlotsAsync(Guid clinicId, DateTime date)
    {
        try
        {
            var url = $"api/timeslots?clinicId={clinicId}&date={date:yyyy-MM-dd}";
            return await _httpClient.GetFromJsonAsync<List<TimeSlotDto>>(url)
                   ?? new List<TimeSlotDto>();
        }
        catch
        {
            return new List<TimeSlotDto>();
        }
    }
}
