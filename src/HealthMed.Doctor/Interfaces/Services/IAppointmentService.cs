using HealthMed.Doctors.Entities;

namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(DateTime dateAppointment, int? doctorId = null);
        Task<Appointment> AcceptAppointment(int appointmentId);
        Task<Appointment> RejectAppointment(int appointmentId);
    }
}
