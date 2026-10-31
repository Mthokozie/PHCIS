namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.Appointments.Commands.BookAppointment;
using Clinic.Application.Features.Appointments.Commands.CancelAppointment;
using Clinic.Application.Features.Appointments.Commands.RescheduleAppointment;
using Clinic.Application.Features.Appointments.Queries.GetAppointmentById;
using Clinic.Application.Features.Appointments.Queries.GetClinicAppointments;
using Clinic.Application.Features.Appointments.Queries.GetPatientAppointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

/// <summary>
/// Endpoints for booking, viewing, cancelling, and rescheduling clinic appointments.
/// </summary>
[Authorize]
[EnableRateLimiting("booking")]
public class AppointmentsController : BaseApiController
{
    /// <summary>Retrieves an appointment by identifier.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AppointmentDto>> GetById(Guid id, CancellationToken ct)
    {
        var appointment = await Mediator.Send(new GetAppointmentByIdQuery(id), ct);
        return Ok(appointment);
    }

    /// <summary>Retrieves all appointments for a specific patient.</summary>
    [HttpGet("patient/{patientId:guid}")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByPatient(Guid patientId, CancellationToken ct)
    {
        var appointments = await Mediator.Send(new GetPatientAppointmentsQuery(patientId), ct);
        return Ok(appointments);
    }

    /// <summary>Retrieves all appointments for a specific clinic (Admin/ClinicStaff).</summary>
    [HttpGet("clinic/{clinicId:guid}")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByClinic(Guid clinicId, CancellationToken ct)
    {
        var appointments = await Mediator.Send(new GetClinicAppointmentsQuery(clinicId), ct);
        return Ok(appointments);
    }

    /// <summary>Books a new clinic appointment and returns a confirmation code.</summary>
    [HttpPost]
    public async Task<ActionResult<AppointmentConfirmationDto>> Book(
        [FromBody] BookAppointmentCommand command,
        CancellationToken ct)
    {
        var confirmation = await Mediator.Send(command, ct);
        return Ok(confirmation);
    }

    /// <summary>Cancels an existing appointment and releases its time slot capacity.</summary>
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelAppointmentRequest request,
        CancellationToken ct)
    {
        await Mediator.Send(new CancelAppointmentCommand(id, request.Reason), ct);
        return Ok(new { message = "Appointment cancelled successfully." });
    }

    /// <summary>Reschedules an existing appointment to a new time slot.</summary>
    [HttpPut("{id:guid}/reschedule")]
    public async Task<ActionResult<AppointmentConfirmationDto>> Reschedule(
        Guid id,
        [FromBody] RescheduleAppointmentRequest request,
        CancellationToken ct)
    {
        var confirmation = await Mediator.Send(
            new RescheduleAppointmentCommand(id, request.NewTimeSlotId, request.Reason),
            ct);
        return Ok(confirmation);
    }
}

/// <summary>Request payload for cancelling an appointment.</summary>
public record CancelAppointmentRequest(string Reason);

/// <summary>Request payload for rescheduling an appointment.</summary>
public record RescheduleAppointmentRequest(Guid NewTimeSlotId, string Reason);
