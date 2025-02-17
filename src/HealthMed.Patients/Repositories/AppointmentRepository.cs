using HealthMed.Patients.Context;
using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Shared.Repos;
using HealthMed.Shared.Repositories;

namespace HealthMed.Patients.Repositories
{
    public class AppointmentRepository : GenericRepositoryDIO<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthMedPatientsDbContext context) : base(context)
        {
        }
    }
}
