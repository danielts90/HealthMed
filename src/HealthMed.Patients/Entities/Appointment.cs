using HealthMed.Shared.Entities;
using HealthMed.Shared.Enum;

namespace HealthMed.Patients.Entities
{
    public class Appointment : EntityBase 
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DoctorMedicalSpeciality Speciality { get; set; }
        public DateTime DateAppointment { get; set; }
        public AppointmentStatus Status { get; set; }
        public string CancelReason { get; set; }
        public double Price { get; set; }

        public Patient Patient { get; set; }
    }
}
