using System.Net.Http.Json;
using System.Text.Json;
using Clinic.Web.Models.Auth;
using Microsoft.JSInterop;

namespace Clinic.Web.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "auth_token";

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<(bool Success, string? ErrorMessage)> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse?.Token != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorageInterop.setItem", TokenKey, loginResponse.Token);
                    return (true, null);
                }
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, $"Login failed: {response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, $"Registration failed: {response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}");
        }
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", TokenKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var payload = ParseJwtPayload(token);
            if (payload.TryGetValue("exp", out var expValue))
            {
                var exp = expValue.GetInt64();
                var expiry = DateTimeOffset.FromUnixTimeSeconds(exp);
                return expiry > DateTimeOffset.UtcNow;
            }
        }
        catch
        {
            // ignored
        }
        return false;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorageInterop.getItem", TokenKey);
    }

    public async Task<string?> GetRoleAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        try
        {
            var payload = ParseJwtPayload(token);
            // Try standard role claim
            if (payload.TryGetValue("role", out var role))
                return role.GetString();
            // Try Microsoft-style claim
            if (payload.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var msRole))
                return msRole.GetString();
        }
        catch { }
        return null;
    }

    public async Task<Guid?> GetPatientIdAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        try
        {
            var payload = ParseJwtPayload(token);
            // Try nameid or sub claim
            if (payload.TryGetValue("nameid", out var nameid) && Guid.TryParse(nameid.GetString(), out var guid1))
                return guid1;
            if (payload.TryGetValue("sub", out var sub) && Guid.TryParse(sub.GetString(), out var guid2))
                return guid2;
            if (payload.TryGetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var msNameid)
                && Guid.TryParse(msNameid.GetString(), out var guid3))
                return guid3;
        }
        catch { }
        return null;
    }

    private static Dictionary<string, JsonElement> ParseJwtPayload(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3)
            throw new ArgumentException("Invalid JWT token");

        var payload = parts[1];
        // Add padding if needed
        payload = payload.Replace('-', '+').Replace('_', '/');
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var bytes = Convert.FromBase64String(payload);
        var json = System.Text.Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)
               ?? new Dictionary<string, JsonElement>();
    }
}
