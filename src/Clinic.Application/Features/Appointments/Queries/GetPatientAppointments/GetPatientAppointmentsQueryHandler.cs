namespace Clinic.Application.Features.Appointments.Queries.GetPatientAppointments;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetPatientAppointmentsQuery"/>.
/// </summary>
public class GetPatientAppointmentsQueryHandler : IRequestHandler<GetPatientAppointmentsQuery, IReadOnlyList<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetPatientAppointmentsQueryHandler"/>.</summary>
    public GetPatientAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AppointmentDto>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetByPatientIdAsync(request.PatientId, cancellationToken);
        return _mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);
    }
}
