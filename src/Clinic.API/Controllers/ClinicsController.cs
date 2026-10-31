namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.Clinics.Commands.CreateClinic;
using Clinic.Application.Features.Clinics.Queries.GetAllClinics;
using Clinic.Application.Features.Clinics.Queries.GetClinicById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for browsing and managing PHCIS healthcare clinics.
/// </summary>
public class ClinicsController : BaseApiController
{
    /// <summary>Retrieves all active clinics.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<ClinicDto>>> GetAll(CancellationToken ct)
    {
        var clinics = await Mediator.Send(new GetAllClinicsQuery(), ct);
        return Ok(clinics);
    }

    /// <summary>Retrieves a single clinic by identifier.</summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ClinicDto>> GetById(Guid id, CancellationToken ct)
    {
        var clinic = await Mediator.Send(new GetClinicByIdQuery(id), ct);
        return Ok(clinic);
    }

    /// <summary>Registers a new healthcare clinic (Admin only).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateClinicCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
}
