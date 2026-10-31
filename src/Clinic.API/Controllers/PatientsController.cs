namespace Clinic.API.Controllers;

using System.Security.Claims;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Application.Features.Patients.Commands.CreatePatient;
using Clinic.Application.Features.Patients.Commands.UpdatePatient;
using Clinic.Application.Features.Patients.Queries.GetAllPatients;
using Clinic.Application.Features.Patients.Queries.GetPatientById;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Manages patient profiles within the PHCIS Clinic Booking System.
/// </summary>
[Authorize]
public class PatientsController : BaseApiController
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="PatientsController"/>.</summary>
    public PatientsController(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    /// <summary>Retrieves all registered patients (Admin/ClinicStaff).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetAll(CancellationToken ct)
    {
        var patients = await Mediator.Send(new GetAllPatientsQuery(), ct);
        return Ok(patients);
    }

    /// <summary>Retrieves a specific patient by identifier (with fallback to the authenticated user's PatientId or Email).</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientDto>> GetById(Guid id, CancellationToken ct)
    {
        var patient = await Mediator.Send(new GetPatientByIdQuery(id), ct);
        if (patient is not null)
        {
            return Ok(patient);
        }

        // Fallback if client passed ApplicationUser.Id (sub) instead of PatientId claim
        var claimPatientId = User.FindFirstValue("PatientId");
        if (Guid.TryParse(claimPatientId, out var resolvedPatientId) && resolvedPatientId != id)
        {
            patient = await Mediator.Send(new GetPatientByIdQuery(resolvedPatientId), ct);
            if (patient is not null)
            {
                return Ok(patient);
            }
        }

        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
        if (!string.IsNullOrWhiteSpace(email))
        {
            var byEmail = await _patientRepository.GetByEmailAsync(email, ct);
            if (byEmail is not null)
            {
                return Ok(_mapper.Map<PatientDto>(byEmail));
            }
        }

        return NotFound(new { message = $"Patient '{id}' was not found." });
    }

    /// <summary>Creates a new patient record.</summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePatientCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Updates an existing patient's contact details and profile photo.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientCommand command, CancellationToken ct)
    {
        var effectiveCommand = command.Id == Guid.Empty ? command with { Id = id } : command;
        await Mediator.Send(effectiveCommand, ct);
        return NoContent();
    }
}
