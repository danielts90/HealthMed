namespace HealthMed.Shared.Dtos
{
    public record AppointmentMessage
    {
        public int PatientAppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateAppointment { get; set; }
        public string PatientName { get; set; }
    }
}
