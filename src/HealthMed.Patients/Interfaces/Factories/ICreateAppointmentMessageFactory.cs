using HealthMed.Patients.Entities;
using HealthMed.Shared.Dtos;

namespace HealthMed.Patients.Interfaces.Factories
{
    public interface ICreateAppointmentMessageFactory
    {
        AppointmentMessage CreateMessage(Appointment appointment, Patient patient);
    }
}
