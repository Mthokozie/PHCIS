namespace Clinic.API.Controllers;

using Clinic.Application.DTOs;
using Clinic.Application.Features.Appointments.Commands.AcceptAppointment;
using Clinic.Application.Features.Appointments.Commands.BookAppointment;
using Clinic.Application.Features.Appointments.Commands.CancelAppointment;
using Clinic.Application.Features.Appointments.Commands.CompleteConsultation;
using Clinic.Application.Features.Appointments.Commands.ProcessAppointment;
using Clinic.Application.Features.Appointments.Commands.ReferAppointmentToHospital;
using Clinic.Application.Features.Appointments.Commands.RescheduleAppointment;
using Clinic.Application.Features.Appointments.Commands.UpdateConsultationNotes;
using Clinic.Application.Features.Appointments.Queries.GetAppointmentById;
using Clinic.Application.Features.Appointments.Queries.GetClinicAppointments;
using Clinic.Application.Features.Appointments.Queries.GetPatientAppointments;
using Clinic.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

/// <summary>
/// Endpoints for booking, viewing, cancelling, rescheduling, accepting, processing,
/// completing, and hospital referral of clinic appointments.
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
        try
        {
            var confirmation = await Mediator.Send(command, ct);
            return Ok(confirmation);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Booking Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Clinic staff accepts an incoming appointment and assigns themselves.</summary>
    [HttpPut("{id:guid}/accept")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<AppointmentDto>> Accept(
        Guid id,
        [FromBody] AcceptAppointmentRequest request,
        CancellationToken ct)
    {
        try
        {
            var appointment = await Mediator.Send(new AcceptAppointmentCommand(id, request.StaffId), ct);
            return Ok(appointment);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Acceptance Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Clinic staff starts processing an appointment (commences consultation).</summary>
    [HttpPut("{id:guid}/process")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<AppointmentDto>> Process(
        Guid id,
        [FromBody] ProcessAppointmentRequest request,
        CancellationToken ct)
    {
        try
        {
            var appointment = await Mediator.Send(new ProcessAppointmentCommand(id, request.StaffId), ct);
            return Ok(appointment);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Processing Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Clinic staff completes a consultation and saves optional notes.</summary>
    [HttpPut("{id:guid}/complete")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<AppointmentDto>> Complete(
        Guid id,
        [FromBody] CompleteConsultationRequest request,
        CancellationToken ct)
    {
        try
        {
            var appointment = await Mediator.Send(new CompleteConsultationCommand(id, request.StaffId, request.Notes), ct);
            return Ok(appointment);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Completion Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Clinic staff refers the consultation to a hospital.</summary>
    [HttpPut("{id:guid}/refer")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<AppointmentDto>> Refer(
        Guid id,
        [FromBody] ReferHospitalRequest request,
        CancellationToken ct)
    {
        try
        {
            var appointment = await Mediator.Send(new ReferAppointmentToHospitalCommand(
                id,
                request.StaffId,
                request.HospitalName,
                request.ReferralReason,
                request.Notes), ct);
            return Ok(appointment);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Referral Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Clinic staff updates or records clinical notes during consultation.</summary>
    [HttpPut("{id:guid}/notes")]
    [Authorize(Roles = "Admin,ClinicStaff")]
    public async Task<ActionResult<AppointmentDto>> UpdateNotes(
        Guid id,
        [FromBody] UpdateConsultationNotesRequest request,
        CancellationToken ct)
    {
        try
        {
            var appointment = await Mediator.Send(new UpdateConsultationNotesCommand(
                id,
                request.StaffId,
                request.Notes), ct);
            return Ok(appointment);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Consultation Notes Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Cancels an existing appointment and releases its time slot capacity.</summary>
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelAppointmentRequest request,
        CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new CancelAppointmentCommand(id, request.Reason), ct);
            return Ok(new { message = "Appointment cancelled successfully." });
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Cancellation Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Reschedules an existing appointment to a new time slot.</summary>
    [HttpPut("{id:guid}/reschedule")]
    public async Task<ActionResult<AppointmentConfirmationDto>> Reschedule(
        Guid id,
        [FromBody] RescheduleAppointmentRequest request,
        CancellationToken ct)
    {
        try
        {
            var confirmation = await Mediator.Send(
                new RescheduleAppointmentCommand(id, request.NewTimeSlotId, request.Reason),
                ct);
            return Ok(confirmation);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Reschedule Rule Violation",
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            });
        }
    }
}

/// <summary>Request payload for accepting an appointment.</summary>
public record AcceptAppointmentRequest(Guid StaffId);

/// <summary>Request payload for processing an appointment.</summary>
public record ProcessAppointmentRequest(Guid StaffId);

/// <summary>Request payload for completing consultation.</summary>
public record CompleteConsultationRequest(Guid StaffId, string? Notes = null);

/// <summary>Request payload for referring consultation to hospital.</summary>
public record ReferHospitalRequest(Guid StaffId, string HospitalName, string ReferralReason, string? Notes = null);

/// <summary>Request payload for updating notes during consultation.</summary>
public record UpdateConsultationNotesRequest(Guid StaffId, string Notes);

/// <summary>Request payload for cancelling an appointment.</summary>
public record CancelAppointmentRequest(string Reason);

/// <summary>Request payload for rescheduling an appointment.</summary>
public record RescheduleAppointmentRequest(Guid NewTimeSlotId, string Reason);
