using HealthMed.Patients.Entities;
using HealthMed.Shared.Enum;

namespace HealthMed.Patients.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointments();
        Task<Appointment> CancelAppointment(int AppointmentId, string cancelReason);
        Task<Appointment> AppointmentUpdatedDoctor(int apppointmentId, AppointmentStatus status);
    }
}
