namespace Clinic.Application.Features.Patients.Queries.GetPatientById;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="GetPatientByIdQuery"/>.
/// </summary>
public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetPatientByIdQueryHandler"/>.</summary>
    public GetPatientByIdQueryHandler(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Patient), request.Id);

        return _mapper.Map<PatientDto>(patient);
    }
}
