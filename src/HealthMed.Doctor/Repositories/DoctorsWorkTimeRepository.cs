using HealthMed.Doctors.Context;
using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Shared.Repositories;

namespace HealthMed.Doctors.Repositories
{
    public class DoctorsWorkTimeRepository : GenericRepository<DoctorsWorkTime>, IDoctorsWorkTimeRepository
    {
        public DoctorsWorkTimeRepository(HealthMedDoctorsDbContext context) : base(context)
        {
        }
    }
}
