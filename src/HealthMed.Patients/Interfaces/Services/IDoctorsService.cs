using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;

namespace HealthMed.Patients.Interfaces.Services
{
    public interface IDoctorsService
    {
        Task<IEnumerable<DoctorsDto>> GetDoctors(DoctorMedicalSpeciality speciality);
        Task<DoctorScheduleDto> GetDoctorFreeTime(int doctorId, DateTime dateAppointment);
    }
}
