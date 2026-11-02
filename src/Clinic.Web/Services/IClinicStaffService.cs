using Clinic.Web.Models.Staff;

namespace Clinic.Web.Services;

public interface IClinicStaffService
{
    Task<ClinicStaffDto?> GetStaffByIdAsync(Guid staffId);
    Task<List<ClinicStaffDto>> GetStaffByClinicAsync(Guid clinicId);
    Task<(bool Success, string? ErrorMessage)> RegisterStaffAsync(RegisterStaffRequest request);
}
