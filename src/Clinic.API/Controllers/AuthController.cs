namespace Clinic.API.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clinic.Application.Features.ClinicStaff.Commands.CreateClinicStaff;
using Clinic.Application.Features.Patients.Commands.CreatePatient;
using Clinic.Domain.Enums;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Handles patient/staff authentication and registration.
/// </summary>
[EnableRateLimiting("auth")]
public class AuthController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    /// <summary>Initialises a new instance of <see cref="AuthController"/>.</summary>
    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    /// <summary>Registers a new patient account and linked patient profile (including optional profile image).</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return BadRequest(new { message = "An account with this email already exists." });
        }

        var patientId = await Mediator.Send(new CreatePatientCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.DateOfBirth,
            request.IdNumber,
            request.ProfileImageUrl), ct);

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = "Patient",
            PatientId = patientId,
            ProfileImageUrl = request.ProfileImageUrl
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { message = "Registered successfully", patientId });
    }

    /// <summary>Registers a new clinic staff member (Nurse, Clerk, Doctor, etc.) and creates their login credentials (Admin only).</summary>
    [HttpPost("register-staff")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffRequest request, CancellationToken ct)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return BadRequest(new { message = "An account with this email already exists." });
        }

        // 1. Create the ClinicStaff domain entity record
        var staffDto = await Mediator.Send(new CreateClinicStaffCommand(
            request.ClinicId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Role,
            request.StaffRegistrationNumber), ct);

        // 2. Create the Identity login user linked to the staff profile
        var identityRole = request.Role == StaffRole.Manager ? "Admin" : "ClinicStaff";
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = identityRole,
            ClinicStaffId = staffDto.Id,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        await _userManager.AddToRoleAsync(user, identityRole);
        if (identityRole == "Admin")
        {
            await _userManager.AddToRoleAsync(user, "ClinicStaff");
        }

        return Ok(new
        {
            message = "Clinic staff member registered successfully",
            staffId = staffDto.Id,
            email = user.Email,
            role = user.Role,
            clinicId = staffDto.ClinicId,
            clinicName = staffDto.ClinicName
        });
    }

    /// <summary>Authenticates a user and returns a signed JWT token.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var expiryHours = int.Parse(_configuration["JwtSettings:ExpiryHours"] ?? "8");
        var expiry = DateTime.UtcNow.AddHours(expiryHours);
        var token = GenerateJwtToken(user, expiry);

        return Ok(new
        {
            token,
            expiry,
            email = user.Email,
            role = user.Role,
            patientId = user.PatientId,
            clinicStaffId = user.ClinicStaffId
        });
    }

    private string GenerateJwtToken(ApplicationUser user, DateTime expiry)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, string.IsNullOrEmpty(user.Role) ? "Patient" : user.Role),
            new("PatientId", user.PatientId?.ToString() ?? string.Empty),
            new("ClinicStaffId", user.ClinicStaffId?.ToString() ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>Request payload for patient registration.</summary>
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    DateTime DateOfBirth,
    string IdNumber,
    string? ProfileImageUrl = null);

/// <summary>Request payload for registering clinic staff (Nurse, Clerk, Doctor).</summary>
public record RegisterStaffRequest(
    Guid ClinicId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    StaffRole Role,
    string StaffRegistrationNumber);

/// <summary>Request payload for user login.</summary>
public record LoginRequest(string Email, string Password);
