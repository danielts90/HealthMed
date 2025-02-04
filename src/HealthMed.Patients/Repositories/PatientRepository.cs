using HealthMed.Patients.Context;
using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Shared.Repositories;

namespace HealthMed.Patients.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(HealthMedPatientsDbContext context) : base(context)
        {
        }
    }
}
