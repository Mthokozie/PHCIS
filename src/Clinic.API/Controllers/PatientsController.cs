namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.Patients.Commands.CreatePatient;
using Clinic.Application.Features.Patients.Commands.UpdatePatient;
using Clinic.Application.Features.Patients.Queries.GetAllPatients;
using Clinic.Application.Features.Patients.Queries.GetPatientById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Manages patient profiles within the PHCIS Clinic Booking System.
/// </summary>
[Authorize]
public class PatientsController : BaseApiController
{
    /// <summary>Retrieves all registered patients (Admin/ClinicStaff).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetAll(CancellationToken ct)
    {
        var patients = await Mediator.Send(new GetAllPatientsQuery(), ct);
        return Ok(patients);
    }

    /// <summary>Retrieves a specific patient by identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientDto>> GetById(Guid id, CancellationToken ct)
    {
        var patient = await Mediator.Send(new GetPatientByIdQuery(id), ct);
        return Ok(patient);
    }

    /// <summary>Creates a new patient record.</summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePatientCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Updates an existing patient's contact details.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "Route ID does not match payload ID." });
        }

        await Mediator.Send(command, ct);
        return NoContent();
    }
}
