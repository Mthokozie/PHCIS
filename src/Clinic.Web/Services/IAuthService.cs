using Clinic.Web.Models.Auth;

namespace Clinic.Web.Services;

public interface IAuthService
{
    Task<(bool Success, string? ErrorMessage)> LoginAsync(LoginRequest request);
    Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
    Task<string?> GetRoleAsync();
    Task<Guid?> GetPatientIdAsync();
}
