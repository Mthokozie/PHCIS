namespace Clinic.Application.Features.Appointments.Commands.CancelAppointment;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="CancelAppointmentCommand"/>.
/// </summary>
public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Initialises a new instance of <see cref="CancelAppointmentCommandHandler"/>.</summary>
    public CancelAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        ITimeSlotRepository timeSlotRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _timeSlotRepository = timeSlotRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        appointment.Cancel(request.Reason);
        _appointmentRepository.Update(appointment);

        var slot = appointment.TimeSlot ?? await _timeSlotRepository.GetByIdAsync(appointment.TimeSlotId, cancellationToken);
        if (slot is not null && slot.CurrentBookings > 0)
        {
            slot.Release();
            _timeSlotRepository.Update(slot);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
