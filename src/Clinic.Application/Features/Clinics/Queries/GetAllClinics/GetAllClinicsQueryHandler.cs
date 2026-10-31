namespace Clinic.Application.Features.Clinics.Queries.GetAllClinics;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetAllClinicsQuery"/>.
/// </summary>
public class GetAllClinicsQueryHandler : IRequestHandler<GetAllClinicsQuery, IReadOnlyList<ClinicDto>>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetAllClinicsQueryHandler"/>.</summary>
    public GetAllClinicsQueryHandler(IClinicRepository clinicRepository, IMapper mapper)
    {
        _clinicRepository = clinicRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ClinicDto>> Handle(GetAllClinicsQuery request, CancellationToken cancellationToken)
    {
        var clinics = await _clinicRepository.GetAllActiveAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ClinicDto>>(clinics);
    }
}
