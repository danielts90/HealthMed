using HealthMed.Shared.Entities;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Prototype;
using System.Text.Json;

namespace HealthMed.Patients.Entities
{
    public class Appointment : EntityBase , IPrototype<Appointment>
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DoctorMedicalSpeciality Speciality { get; set; }
        public DateTime DateAppointment { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? CancelReason { get; set; }
        public double Price { get; set; }

        public Patient? Patient { get; set; }

        public Appointment DeepClone() => JsonSerializer.Deserialize<Appointment>(JsonSerializer.Serialize(this))!;


        public Appointment ShallowClone() => (Appointment)this.MemberwiseClone();

    }
}
