using Microsoft.EntityFrameworkCore;
using HealthMed.Doctors.Entities;
using System.Reflection;

namespace HealthMed.Doctors.Context
{
    public class HealthMedDoctorsDbContext : DbContext
    {
        public HealthMedDoctorsDbContext(DbContextOptions<HealthMedDoctorsDbContext> options) : base(options) { }

        DbSet<Doctor> Doctors { get; set; }
        DbSet<DoctorsWorkTime> DoctorsWorkTimes { get; set; }
        DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

    }
}
