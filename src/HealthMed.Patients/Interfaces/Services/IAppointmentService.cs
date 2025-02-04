using HealthMed.Patients.Entities;

namespace HealthMed.Patients.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<Appointment> GetAppointments();
        Task<Appointment> CancelAppointment(Appointment appointment);
    }
}
