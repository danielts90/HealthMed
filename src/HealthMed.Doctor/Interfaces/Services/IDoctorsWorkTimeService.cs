using HealthMed.Doctors.Entities;

namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IDoctorsWorkTimeService
    {
        Task<DoctorsWorkTime> AddWorkTime(int doctorId, DoctorsWorkTime workTime);
        Task<DoctorsWorkTime> UpdateWorkTime(int doctorId, DoctorsWorkTime workTime);
        Task<IEnumerable<DoctorsWorkTime>> GetDoctorWorkTime(int doctorId);
    }
}
