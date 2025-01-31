using HealthMed.Auth.Enum;

namespace HealthMed.Auth.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email  { get; set; }
        public string Password { get; set; }
        public UserType UserType  { get; set; }
    }
}
