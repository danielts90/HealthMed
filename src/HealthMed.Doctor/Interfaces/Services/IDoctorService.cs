using HealthMed.Doctors.Entities;

namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<Doctor> CreateDoctor(Doctor doctor);
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor?> GetDoctorById(int id);
    }
}
