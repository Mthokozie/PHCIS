namespace Clinic.Application.Features.Appointments.Commands.BookAppointment;

using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Command to book an appointment for a patient in a specific time slot.
/// </summary>
public record BookAppointmentCommand(
    Guid PatientId,
    Guid TimeSlotId,
    string Reason) : IRequest<AppointmentConfirmationDto>;
