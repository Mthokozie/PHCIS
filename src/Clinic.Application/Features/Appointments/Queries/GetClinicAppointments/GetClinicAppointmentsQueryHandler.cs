namespace Clinic.Application.Features.Appointments.Queries.GetClinicAppointments;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetClinicAppointmentsQuery"/>.
/// </summary>
public class GetClinicAppointmentsQueryHandler : IRequestHandler<GetClinicAppointmentsQuery, IReadOnlyList<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetClinicAppointmentsQueryHandler"/>.</summary>
    public GetClinicAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AppointmentDto>> Handle(GetClinicAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetByClinicIdAsync(request.ClinicId, cancellationToken);
        return _mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);
    }
}
