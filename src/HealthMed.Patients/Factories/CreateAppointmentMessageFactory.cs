using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Factories;
using HealthMed.Shared.Dtos;

namespace HealthMed.Patients.Factories
{
    public class CreateAppointmentMessageFactory : ICreateAppointmentMessageFactory
    {
        public AppointmentMessage CreateMessage(Appointment appointment, Patient patient)
        {
            return new AppointmentMessage
            {
                PatientAppointmentId = appointment.Id,
                PatientId = patient.Id,
                PatientName = patient.Name,
                DoctorId = appointment.DoctorId,
                DateAppointment = appointment.DateAppointment
            };
        }
    }
}
