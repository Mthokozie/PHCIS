namespace Clinic.Application.Features.Patients.Queries.GetAllPatients;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetAllPatientsQuery"/>.
/// </summary>
public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, IReadOnlyList<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetAllPatientsQueryHandler"/>.</summary>
    public GetAllPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<PatientDto>>(patients);
    }
}
