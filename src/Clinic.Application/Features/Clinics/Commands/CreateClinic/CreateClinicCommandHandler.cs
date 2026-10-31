namespace Clinic.Application.Features.Clinics.Commands.CreateClinic;

using Clinic.Application.Common.Interfaces;
using MediatR;

/// <summary>
/// Handles <see cref="CreateClinicCommand"/>.
/// </summary>
public class CreateClinicCommandHandler : IRequestHandler<CreateClinicCommand, Guid>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Initialises a new instance of <see cref="CreateClinicCommandHandler"/>.</summary>
    public CreateClinicCommandHandler(IClinicRepository clinicRepository, IUnitOfWork unitOfWork)
    {
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Guid> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
    {
        var opening = TimeSpan.Parse(request.OpeningTime);
        var closing = TimeSpan.Parse(request.ClosingTime);

        var clinic = global::Clinic.Domain.Entities.Clinic.Create(
            request.Name,
            request.Address,
            request.Phone,
            request.Email,
            opening,
            closing,
            request.SlotDurationMinutes);

        await _clinicRepository.AddAsync(clinic, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return clinic.Id;
    }
}
