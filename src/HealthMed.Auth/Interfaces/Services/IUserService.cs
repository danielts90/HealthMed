using HealthMed.Auth.Entities;

namespace HealthMed.Auth.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> Login(string username, string password);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
    }
}
