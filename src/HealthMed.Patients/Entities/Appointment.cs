using HealthMed.Shared.Entities;
using HealthMed.Shared.Enum;

namespace HealthMed.Patients.Entities
{
    public class Appointment : EntityBase 
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateAppointment { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
