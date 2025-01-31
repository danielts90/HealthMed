using HealthMed.Auth.Entities;

namespace HealthMed.Auth.Interfaces.Services
{
    public interface IJwtService
    {
        string GetJwtToken(User User);
    }
}
