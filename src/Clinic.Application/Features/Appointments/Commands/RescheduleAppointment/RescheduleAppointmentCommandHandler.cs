namespace Clinic.Application.Features.Appointments.Commands.RescheduleAppointment;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="RescheduleAppointmentCommand"/> by cancelling the existing booking,
/// releasing its slot, reserving the new slot, and creating a new confirmed booking.
/// </summary>
public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, AppointmentConfirmationDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="RescheduleAppointmentCommandHandler"/>.</summary>
    public RescheduleAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        ITimeSlotRepository timeSlotRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _timeSlotRepository = timeSlotRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<AppointmentConfirmationDto> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var oldAppointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        var newSlot = await _timeSlotRepository.GetByIdAsync(request.NewTimeSlotId, cancellationToken)
            ?? throw new NotFoundException(nameof(TimeSlot), request.NewTimeSlotId);

        var alreadyBooked = await _appointmentRepository.HasPatientBookingOnSlotAsync(
            oldAppointment.PatientId,
            request.NewTimeSlotId,
            cancellationToken);

        if (alreadyBooked)
        {
            throw new DomainException("Patient already has an active booking for the requested time slot.");
        }

        // Cancel old appointment & release capacity
        oldAppointment.Cancel($"Rescheduled: {request.Reason}");
        _appointmentRepository.Update(oldAppointment);

        var oldSlot = oldAppointment.TimeSlot ?? await _timeSlotRepository.GetByIdAsync(oldAppointment.TimeSlotId, cancellationToken);
        if (oldSlot is not null && oldSlot.CurrentBookings > 0)
        {
            oldSlot.Release();
            _timeSlotRepository.Update(oldSlot);
        }

        // Reserve new slot & create new appointment
        newSlot.Reserve();
        _timeSlotRepository.Update(newSlot);

        var newAppointment = Appointment.Create(
            oldAppointment.PatientId,
            newSlot.ClinicId,
            newSlot.Id,
            request.Reason);

        newAppointment.Confirm();
        await _appointmentRepository.AddAsync(newAppointment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var confirmation = _mapper.Map<AppointmentConfirmationDto>(newAppointment);
        if (confirmation is not null)
        {
            if (string.IsNullOrEmpty(confirmation.PatientName) && oldAppointment.Patient is not null)
                confirmation.PatientName = oldAppointment.Patient.FullName;
            confirmation.SlotStartTime = newSlot.StartTime;
            confirmation.SlotEndTime = newSlot.EndTime;
        }

        return confirmation!;
    }
}
