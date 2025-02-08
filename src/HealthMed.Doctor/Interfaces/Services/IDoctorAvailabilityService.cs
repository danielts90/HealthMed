using HealthMed.Shared.Dtos;

namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IDoctorAvailabilityService
    {
        Task<DoctorScheduleDto> GetAvailableSlots(int doctorId, DateTime date);
    }
}
