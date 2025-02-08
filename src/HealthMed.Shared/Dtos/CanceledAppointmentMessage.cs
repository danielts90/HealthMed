namespace HealthMed.Shared.Dtos
{
    public record CanceledAppointmentMessage(int PatientAppointmentId, string cancelReason);
}
