namespace Clinic.Application.Features.Appointments.Queries.GetAppointmentById;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="GetAppointmentByIdQuery"/>.
/// </summary>
public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetAppointmentByIdQueryHandler"/>.</summary>
    public GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.Id);

        return _mapper.Map<AppointmentDto>(appointment);
    }
}
