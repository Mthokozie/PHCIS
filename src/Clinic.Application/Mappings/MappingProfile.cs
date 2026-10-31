namespace Clinic.Application.Mappings;

using AutoMapper;
using Clinic.Application.DTOs;
using Clinic.Domain.Entities;

/// <summary>
/// AutoMapper profile defining mappings between Domain entities and Application DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>Initialises a new instance of <see cref="MappingProfile"/>.</summary>
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName));

        CreateMap<global::Clinic.Domain.Entities.Clinic, ClinicDto>()
            .ForMember(d => d.OpeningTime, opt => opt.MapFrom(s => s.OpeningTime.ToString(@"hh\:mm")))
            .ForMember(d => d.ClosingTime, opt => opt.MapFrom(s => s.ClosingTime.ToString(@"hh\:mm")));

        CreateMap<TimeSlot, TimeSlotDto>()
            .ForMember(d => d.ClinicName, opt => opt.MapFrom(s => s.Clinic != null ? s.Clinic.Name : string.Empty))
            .ForMember(d => d.RemainingCapacity, opt => opt.MapFrom(s => s.RemainingCapacity));

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient != null ? s.Patient.FullName : string.Empty))
            .ForMember(d => d.ClinicName, opt => opt.MapFrom(s => s.Clinic != null ? s.Clinic.Name : string.Empty))
            .ForMember(d => d.SlotStartTime, opt => opt.MapFrom(s => s.TimeSlot != null ? s.TimeSlot.StartTime : DateTime.MinValue))
            .ForMember(d => d.SlotEndTime, opt => opt.MapFrom(s => s.TimeSlot != null ? s.TimeSlot.EndTime : DateTime.MinValue))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<Appointment, AppointmentConfirmationDto>()
            .ForMember(d => d.AppointmentId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient != null ? s.Patient.FullName : string.Empty))
            .ForMember(d => d.ClinicName, opt => opt.MapFrom(s => s.Clinic != null ? s.Clinic.Name : string.Empty))
            .ForMember(d => d.SlotStartTime, opt => opt.MapFrom(s => s.TimeSlot != null ? s.TimeSlot.StartTime : DateTime.MinValue))
            .ForMember(d => d.SlotEndTime, opt => opt.MapFrom(s => s.TimeSlot != null ? s.TimeSlot.EndTime : DateTime.MinValue))
            .ForMember(d => d.Message, opt => opt.MapFrom(s => $"Appointment confirmed! Reference code: {s.ConfirmationCode}"));
    }
}
