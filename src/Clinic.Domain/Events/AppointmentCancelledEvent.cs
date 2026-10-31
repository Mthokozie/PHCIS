using Clinic.Domain.Common;

namespace Clinic.Domain.Events;

/// <summary>
/// Domain event raised when an appointment is cancelled.
/// </summary>
/// <param name="AppointmentId">The unique identifier of the cancelled appointment.</param>
/// <param name="Reason">The reason provided for the cancellation.</param>
public record AppointmentCancelledEvent(
    Guid AppointmentId,
    string Reason) : IDomainEvent;
