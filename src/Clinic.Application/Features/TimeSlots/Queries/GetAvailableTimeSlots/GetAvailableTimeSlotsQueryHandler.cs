namespace Clinic.Application.Features.TimeSlots.Queries.GetAvailableTimeSlots;

using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.DTOs;
using MediatR;

/// <summary>
/// Handles <see cref="GetAvailableTimeSlotsQuery"/>.
/// </summary>
public class GetAvailableTimeSlotsQueryHandler : IRequestHandler<GetAvailableTimeSlotsQuery, IReadOnlyList<TimeSlotDto>>
{
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IMapper _mapper;

    /// <summary>Initialises a new instance of <see cref="GetAvailableTimeSlotsQueryHandler"/>.</summary>
    public GetAvailableTimeSlotsQueryHandler(ITimeSlotRepository timeSlotRepository, IMapper mapper)
    {
        _timeSlotRepository = timeSlotRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TimeSlotDto>> Handle(GetAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
    {
        var dateOnly = DateOnly.FromDateTime(request.Date);
        var slots = await _timeSlotRepository.GetAvailableSlotsByClinicAndDateAsync(
            request.ClinicId,
            dateOnly,
            cancellationToken);

        return _mapper.Map<IReadOnlyList<TimeSlotDto>>(slots);
    }
}
