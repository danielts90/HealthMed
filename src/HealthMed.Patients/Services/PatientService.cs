using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.Services;

namespace HealthMed.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }
    }
}
