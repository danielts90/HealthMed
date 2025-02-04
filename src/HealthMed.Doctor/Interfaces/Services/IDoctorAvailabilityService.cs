namespace HealthMed.Doctors.Interfaces.Services
{
    public interface IDoctorAvailabilityService
    {
        Task<IEnumerable<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date);
    }
}
