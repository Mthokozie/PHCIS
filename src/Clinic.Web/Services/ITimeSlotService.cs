using Clinic.Web.Models;

namespace Clinic.Web.Services;

public interface ITimeSlotService
{
    Task<List<TimeSlotDto>> GetAvailableSlotsAsync(Guid clinicId, DateTime date);
}
