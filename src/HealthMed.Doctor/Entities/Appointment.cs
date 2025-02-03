using HealthMed.Shared.Entities;

namespace HealthMed.Doctors.Entities
{
    public class Appointment : EntityBase
    {
        public int DoctorId { get; set; }
        public DateTime DateAppointment { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public Doctor Doctor { get; set; }

    }

}
