using HealthMed.Patients.Entities;
using HealthMed.Shared.Repositories.Interfaces;

namespace HealthMed.Patients.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepositoryDIO<Patient>
    { }
}
