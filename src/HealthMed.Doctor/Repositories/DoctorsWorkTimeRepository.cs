using HealthMed.Doctors.Context;
using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Shared.Repos;
using HealthMed.Shared.Repositories;

namespace HealthMed.Doctors.Repositories
{
    public class DoctorsWorkTimeRepository : GenericRepositoryDIO<DoctorsWorkTime>, IDoctorsWorkTimeRepository
    {
        public DoctorsWorkTimeRepository(HealthMedDoctorsDbContext context) : base(context)
        {
        }
    }
}
