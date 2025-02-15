using HealthMed.Doctors.Consumers;
using MassTransit;

namespace HealthMed.Doctors.Configurations
{
    public static class MassTransiftConfig
    {
        public static void ConfigureMassTransit(this IServiceCollection services, ConfigurationManager configuration) 
        {
            var endpoint = configuration["Rabbit:Endpoint"];

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.AddConsumer<CreateAppointmentConsumer>();
                config.AddConsumer<CanceledAppointmentConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(endpoint, h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("patient-appointment-queue", e =>
                    {
                        e.ConfigureConsumer<CreateAppointmentConsumer>(ctx);
                        e.ConfigureConsumer<CanceledAppointmentConsumer>(ctx);
                    });
                });
            });
        }
    }
}
