using HealthMed.Patients.Entities;

namespace HealthMed.Patients.Interfaces.Services
{
    public interface IPatientService
    {
        Task<Patient> AddPatient(Patient patient);
        Task<Patient> UpdatePatient(Patient patient);
        Task<Patient> GetPatientByUserId();
    }
}
