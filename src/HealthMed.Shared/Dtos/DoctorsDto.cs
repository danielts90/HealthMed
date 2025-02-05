using HealthMed.Shared.Enum;

namespace HealthMed.Shared.Dtos
{
    public class DoctorsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DoctorMedicalSpeciality Speciality { get; set; }

    }
}
