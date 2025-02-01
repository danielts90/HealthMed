using HealthMed.Auth.Context;
using HealthMed.Auth.Entities;
using HealthMed.Auth.Repositories.Interfaces;
using HealthMed.Shared.Repositories;

namespace HealthMed.Auth.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(HealthMedDbContext context) : base(context)
        {
        }
    }
}
