namespace Clinic.Application.Features.Appointments.Commands.UpdateConsultationNotes;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Enums;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="UpdateConsultationNotesCommand"/>.
/// Updates clinical consultation notes while an appointment is in consultation.
/// </summary>
public class UpdateConsultationNotesCommandHandler : IRequestHandler<UpdateConsultationNotesCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClinicStaffRepository _staffRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="UpdateConsultationNotesCommandHandler"/>.</summary>
    public UpdateConsultationNotesCommandHandler(
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
    public async Task<AppointmentDto> Handle(UpdateConsultationNotesCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        var staff = await _staffRepository.GetByIdAsync(request.StaffId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClinicStaff), request.StaffId);

        if (!staff.IsActive)
        {
            throw new DomainException("Inactive staff member cannot update consultation notes.");
        }

        if (staff.ClinicId != appointment.ClinicId && staff.Role != StaffRole.Manager)
        {
            throw new DomainException("Staff member does not belong to the clinic where the appointment is booked.");
        }

        appointment.UpdateConsultationNotes(staff.Id, request.Notes);
        _appointmentRepository.Update(appointment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var refreshedAppointment = await _appointmentRepository.GetByIdAsync(appointment.Id, cancellationToken);
        return _mapper.Map<AppointmentDto>(refreshedAppointment ?? appointment);
    }
}
