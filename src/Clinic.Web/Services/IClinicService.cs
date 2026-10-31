using Clinic.Web.Models;

namespace Clinic.Web.Services;

public interface IClinicService
{
    Task<List<ClinicDto>> GetAllClinicsAsync();
    Task<ClinicDto?> GetClinicByIdAsync(Guid id);
}
