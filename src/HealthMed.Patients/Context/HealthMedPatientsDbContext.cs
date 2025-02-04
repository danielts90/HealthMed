using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HealthMed.Patients.Context
{
    public class HealthMedPatientsDbContext : DbContext
    {
        public HealthMedPatientsDbContext(DbContextOptions<HealthMedPatientsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
