namespace Clinic.Application.Features.ClinicStaff.Queries.GetClinicStaffById;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="GetClinicStaffByIdQuery"/>.
/// </summary>
public class GetClinicStaffByIdQueryHandler : IRequestHandler<GetClinicStaffByIdQuery, ClinicStaffDto>
{
    private readonly IClinicStaffRepository _staffRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetClinicStaffByIdQueryHandler"/>.</summary>
    public GetClinicStaffByIdQueryHandler(IClinicStaffRepository staffRepository, IMapper mapper)
    {
        _staffRepository = staffRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ClinicStaffDto> Handle(GetClinicStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var staff = await _staffRepository.GetByIdAsync(request.StaffId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClinicStaff), request.StaffId);

        return _mapper.Map<ClinicStaffDto>(staff);
    }
}
