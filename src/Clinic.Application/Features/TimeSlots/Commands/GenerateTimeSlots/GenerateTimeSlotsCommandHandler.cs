namespace Clinic.Application.Features.TimeSlots.Commands.GenerateTimeSlots;

using Clinic.Application.Common.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Domain.Exceptions;
using MediatR;

/// <summary>
/// Handles <see cref="GenerateTimeSlotsCommand"/> by generating slots between the clinic's opening and closing times.
/// </summary>
public class GenerateTimeSlotsCommandHandler : IRequestHandler<GenerateTimeSlotsCommand, int>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Initialises a new instance of <see cref="GenerateTimeSlotsCommandHandler"/>.</summary>
    public GenerateTimeSlotsCommandHandler(
        IClinicRepository clinicRepository,
        ITimeSlotRepository timeSlotRepository,
        IUnitOfWork unitOfWork)
    {
        _clinicRepository = clinicRepository;
        _timeSlotRepository = timeSlotRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<int> Handle(GenerateTimeSlotsCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken)
            ?? throw new NotFoundException(nameof(global::Clinic.Domain.Entities.Clinic), request.ClinicId);

        var baseDate = request.Date.Date;
        var currentStart = baseDate.Add(clinic.OpeningTime);
        var closingDateTime = baseDate.Add(clinic.ClosingTime);
        var slotDuration = TimeSpan.FromMinutes(clinic.SlotDurationMinutes);

        var slots = new List<TimeSlot>();
        while (currentStart.Add(slotDuration) <= closingDateTime)
        {
            var slotEnd = currentStart.Add(slotDuration);
            slots.Add(TimeSlot.Create(clinic.Id, currentStart, slotEnd, request.MaxCapacityPerSlot));
            currentStart = slotEnd;
        }

        if (slots.Count > 0)
        {
            await _timeSlotRepository.AddRangeAsync(slots, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return slots.Count;
    }
}
