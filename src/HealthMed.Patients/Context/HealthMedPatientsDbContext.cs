using HealthMed.Patients.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HealthMed.Patients.Context
{
    public class HealthMedPatientsDbContext : DbContext
    {
        public HealthMedPatientsDbContext(DbContextOptions<HealthMedPatientsDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
