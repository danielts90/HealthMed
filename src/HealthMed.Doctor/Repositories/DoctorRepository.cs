using HealthMed.Doctors.Context;
using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Shared.Repos;
using HealthMed.Shared.Repositories;

namespace HealthMed.Doctors.Repositories
{
    public class DoctorRepository : GenericRepositoryDIO<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealthMedDoctorsDbContext context) : base(context)
        {
        }
    }
}
