namespace HealthMed.Shared.Dtos
{
    public class DoctorScheduleDto
    {
        public List<TimeSpan> Times { get; set; } = new List<TimeSpan>();
        public double Price { get; set; }
    }
}
