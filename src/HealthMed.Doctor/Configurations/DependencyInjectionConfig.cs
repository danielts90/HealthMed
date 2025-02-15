using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Repositories;
using HealthMed.Doctors.Services;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorsWorkTimeRepository, DoctorsWorkTimeRepository>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorsWorkTimeService, DoctorsWorkTimeService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
