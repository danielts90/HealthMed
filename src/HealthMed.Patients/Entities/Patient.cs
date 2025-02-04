using HealthMed.Shared.Entities;

namespace HealthMed.Patients.Entities
{
    public class Patient : EntityBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}
