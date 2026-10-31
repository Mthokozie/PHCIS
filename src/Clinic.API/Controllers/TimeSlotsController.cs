namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.TimeSlots.Commands.GenerateTimeSlots;
using Clinic.Application.Features.TimeSlots.Queries.GetAvailableTimeSlots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for querying available clinic time slots and generating daily schedules.
/// </summary>
public class TimeSlotsController : BaseApiController
{
    /// <summary>Retrieves available time slots for a clinic on a given date.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TimeSlotDto>>> GetAvailable(
        [FromQuery] Guid clinicId,
        [FromQuery] DateTime date,
        CancellationToken ct)
    {
        var slots = await Mediator.Send(new GetAvailableTimeSlotsQuery(clinicId, date), ct);
        return Ok(slots);
    }

    /// <summary>Generates bookable time slots for a clinic on a specified date.</summary>
    [HttpPost("generate")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<int>> Generate([FromBody] GenerateTimeSlotsCommand command, CancellationToken ct)
    {
        var count = await Mediator.Send(command, ct);
        return Ok(new { slotsGenerated = count });
    }
}
