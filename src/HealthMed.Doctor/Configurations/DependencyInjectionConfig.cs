using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Interfaces.UnitOfWork;
using HealthMed.Doctors.Repositories;
using HealthMed.Doctors.Services;
using HealthMed.Doctors.UnitOfWorks;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorsWorkTimeService, DoctorsWorkTimeService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorsWorkTimeRepository, DoctorsWorkTimeRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        }
    }
}
