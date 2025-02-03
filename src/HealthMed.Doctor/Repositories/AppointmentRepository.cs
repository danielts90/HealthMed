using HealthMed.Doctors.Context;
using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Shared.Repositories;

namespace HealthMed.Doctors.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthMedDoctorsDbContext context) : base(context)
        {
        }
    }
}
