using HealthMed.Shared.Enum;

namespace HealthMed.Shared.Dtos
{
    public record AppointmentDoctorUpdateMessage(int PatientAppointmentId, AppointmentStatus Status);
}
