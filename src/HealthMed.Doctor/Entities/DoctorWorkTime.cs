using HealthMed.Shared.Entities;

namespace HealthMed.Doctors.Entities
{
    public class DoctorsWorkTime : EntityBase
    {
        public int DoctorId { get; set; }
        public int WeekDay { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan StartInterval { get; set; }
        public TimeSpan FinishInterval { get; set; }
        public TimeSpan ExitTime { get; set; }
        public int AppointmentDuration { get; set; }

        // Relacionamento com Doctor
        public Doctor Doctor { get; set; }
    }

}
