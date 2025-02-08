using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Shared.MassTransit
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.AddConsumers(typeof(MassTransitConfig).Assembly);

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
