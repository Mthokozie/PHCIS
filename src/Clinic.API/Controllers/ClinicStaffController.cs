namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.ClinicStaff.Commands.CreateClinicStaff;
using Clinic.Application.Features.ClinicStaff.Queries.GetClinicStaffById;
using Clinic.Application.Features.ClinicStaff.Queries.GetStaffByClinic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for managing internal clinic staff members.
/// </summary>
[Authorize(Roles = "Admin,ClinicStaff")]
public class ClinicStaffController : BaseApiController
{
    /// <summary>Retrieves a clinic staff member by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClinicStaffDto>> GetById(Guid id, CancellationToken ct)
    {
        var staff = await Mediator.Send(new GetClinicStaffByIdQuery(id), ct);
        return Ok(staff);
    }

    /// <summary>Retrieves all internal staff members assigned to a clinic.</summary>
    [HttpGet("clinic/{clinicId:guid}")]
    public async Task<ActionResult<IReadOnlyList<ClinicStaffDto>>> GetByClinic(Guid clinicId, CancellationToken ct)
    {
        var staff = await Mediator.Send(new GetStaffByClinicQuery(clinicId), ct);
        return Ok(staff);
    }

    /// <summary>Onboards and registers a new internal staff member for a clinic.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClinicStaffDto>> Create([FromBody] CreateClinicStaffCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
