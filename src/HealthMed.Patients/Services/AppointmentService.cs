using HealthMed.Patients.Interfaces.Services;

namespace HealthMed.Patients.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentService(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
    }
}
