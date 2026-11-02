namespace Clinic.Application.Features.ClinicStaff.Commands.CreateClinicStaff;

using AutoMapper;
using Clinic.Application.Common.Exceptions;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="CreateClinicStaffCommand"/>.
/// </summary>
public class CreateClinicStaffCommandHandler : IRequestHandler<CreateClinicStaffCommand, ClinicStaffDto>
{
    private readonly IClinicStaffRepository _staffRepository;
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="CreateClinicStaffCommandHandler"/>.</summary>
    public CreateClinicStaffCommandHandler(
        IClinicStaffRepository staffRepository,
        IClinicRepository clinicRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _staffRepository = staffRepository;
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ClinicStaffDto> Handle(CreateClinicStaffCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken)
            ?? throw new NotFoundException(nameof(global::Clinic.Domain.Entities.Clinic), request.ClinicId);

        var existingWithEmail = await _staffRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingWithEmail != null)
        {
            throw new ConflictException($"A clinic staff member with email '{request.Email}' already exists.");
        }

        var staff = ClinicStaff.Create(
            clinic.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Role,
            request.StaffRegistrationNumber);

        await _staffRepository.AddAsync(staff, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdStaff = await _staffRepository.GetByIdAsync(staff.Id, cancellationToken);
        return _mapper.Map<ClinicStaffDto>(createdStaff ?? staff);
    }
}
