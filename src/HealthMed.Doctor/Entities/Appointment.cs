using HealthMed.Shared.Entities;
using HealthMed.Shared.Enum;

namespace HealthMed.Doctors.Entities
{
    public class Appointment : EntityBase
    {
        public int DoctorId { get; set; }
        public DateTime DateAppointment { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public AppointmentStatus Status { get; set; }
        public int PatientAppointmentId { get; set; }
        public string? CancelReason { get; set; }
        public Doctor? Doctor { get; set; }

    }

}
