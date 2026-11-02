namespace Clinic.Application.Features.ClinicStaff.Queries.GetStaffByClinic;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetStaffByClinicQuery"/>.
/// </summary>
public class GetStaffByClinicQueryHandler : IRequestHandler<GetStaffByClinicQuery, IReadOnlyList<ClinicStaffDto>>
{
    private readonly IClinicStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetStaffByClinicQueryHandler"/>.</summary>
    public GetStaffByClinicQueryHandler(IClinicStaffRepository staffRepository, IMapper mapper)
    {
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ClinicStaffDto>> Handle(GetStaffByClinicQuery request, CancellationToken cancellationToken)
    {
        var staffList = await _staffRepository.GetByClinicIdAsync(request.ClinicId, cancellationToken);
        return _mapper.Map<IReadOnlyList<ClinicStaffDto>>(staffList);
    }
}
