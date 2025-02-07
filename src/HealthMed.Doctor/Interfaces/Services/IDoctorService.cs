using HealthMed.Doctors.Entities;
using HealthMed.Shared.Enum;

namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<Doctor> CreateDoctor(Doctor doctor);
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor?> GetDoctorById(int id);
        Task<IEnumerable<Doctor?>> GetDoctorBySpeciallity(DoctorMedicalSpeciality speciality);
    }
}
