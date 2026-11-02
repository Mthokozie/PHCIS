namespace Clinic.Application.Features.Appointments.Commands.CompleteConsultation;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="CompleteConsultationCommand"/>.
/// Marks consultation as complete, saves clinical notes, and stamps completion timestamp.
/// </summary>
public class CompleteConsultationCommandHandler : IRequestHandler<CompleteConsultationCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClinicStaffRepository _staffRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="CompleteConsultationCommandHandler"/>.</summary>
    public CompleteConsultationCommandHandler(
        IAppointmentRepository appointmentRepository,
        IClinicStaffRepository staffRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _staffRepository = staffRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<AppointmentDto> Handle(CompleteConsultationCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        var staff = await _staffRepository.GetByIdAsync(request.StaffId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClinicStaff), request.StaffId);

        if (!staff.IsActive)
        {
            throw new DomainException("Inactive staff member cannot complete consultations.");
        }

        if (staff.ClinicId != appointment.ClinicId && staff.Role != StaffRole.Manager)
        {
            throw new DomainException("Staff member does not belong to the clinic where the appointment is booked.");
        }

        appointment.MarkAsCompleted(staff.Id, request.Notes);
        _appointmentRepository.Update(appointment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var refreshedAppointment = await _appointmentRepository.GetByIdAsync(appointment.Id, cancellationToken);
        return _mapper.Map<AppointmentDto>(refreshedAppointment ?? appointment);
    }
}
