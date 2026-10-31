using Clinic.Web.Models;
using Clinic.Web.Models.Patients;

namespace Clinic.Web.Services;

public interface IPatientService
{
    Task<PatientDto?> GetPatientByIdAsync(Guid id);
    Task<bool> UpdatePatientAsync(UpdatePatientRequest request);
}
