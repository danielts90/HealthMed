using HealthMed.Shared.Entities;

namespace HealthMed.Doctors.Entities
{
    public class Doctor : EntityBase
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? CRM { get; set; }
        public string? CPF { get; set; }
    }
}
