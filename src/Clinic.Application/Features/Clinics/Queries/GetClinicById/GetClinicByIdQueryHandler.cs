namespace Clinic.Application.Features.Clinics.Queries.GetClinicById;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="GetClinicByIdQuery"/>.
/// </summary>
public class GetClinicByIdQueryHandler : IRequestHandler<GetClinicByIdQuery, ClinicDto>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetClinicByIdQueryHandler"/>.</summary>
    public GetClinicByIdQueryHandler(IClinicRepository clinicRepository, IMapper mapper)
    {
        _clinicRepository = clinicRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ClinicDto> Handle(GetClinicByIdQuery request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(global::Clinic.Domain.Entities.Clinic), request.Id);

        return _mapper.Map<ClinicDto>(clinic);
    }
}
