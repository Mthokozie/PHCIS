namespace Clinic.Application.Features.Appointments.Commands.BookAppointment;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="BookAppointmentCommand"/> by enforcing capacity and double-booking rules.
/// </summary>
public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, AppointmentConfirmationDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="BookAppointmentCommandHandler"/>.</summary>
    public BookAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        ITimeSlotRepository timeSlotRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _timeSlotRepository = timeSlotRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<AppointmentConfirmationDto> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.PatientId);

        var slot = await _timeSlotRepository.GetByIdAsync(request.TimeSlotId, cancellationToken)
            ?? throw new NotFoundException(nameof(TimeSlot), request.TimeSlotId);

        var alreadyBooked = await _appointmentRepository.HasPatientBookingOnSlotAsync(
            request.PatientId,
            request.TimeSlotId,
            cancellationToken);

        if (alreadyBooked)
        {
            throw new DomainException("Patient already has an active booking for this time slot.");
        }

        // Reserve capacity on the slot (throws DomainException if full)
        slot.Reserve();

        var appointment = Appointment.Create(
            patient.Id,
            slot.ClinicId,
            request.TimeSlotId,
            request.Reason);

        appointment.Confirm();

        await _appointmentRepository.AddAsync(appointment, cancellationToken);
        _timeSlotRepository.Update(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var confirmation = _mapper.Map<AppointmentConfirmationDto>(appointment);
        if (confirmation is not null)
        {
            if (string.IsNullOrEmpty(confirmation.PatientName))
                confirmation.PatientName = patient.FullName;
            if (confirmation.SlotStartTime == default)
                confirmation.SlotStartTime = slot.StartTime;
            if (confirmation.SlotEndTime == default)
                confirmation.SlotEndTime = slot.EndTime;
        }

        return confirmation!;
    }
}
