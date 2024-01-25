using RPS.Application.Configs;
using MassTransit;

namespace RPS.API.ServicesExtensions.MassTransit;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitConfiguration = new RabbitMqConfig
        {
            Hostname = configuration["MessageBroker:Hostname"]!,
            Password = configuration["MessageBroker:Password"]!,
            Username = configuration["MessageBroker:Username"]!,
            Port = configuration["MessageBroker:Port"]!
        };
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var uri =
                    $"amqp://{rabbitConfiguration.Username}:{rabbitConfiguration.Password}@{rabbitConfiguration.Hostname}:{rabbitConfiguration.Port}";
                configurator.Host(uri);
                configurator.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}