using RPS.Application.Clients.MongoClient;
using RPS.Application.Configs;
using RPS.Application.Services.Abstractions;

namespace RPS.API.ServicesExtensions.Mongo;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongo(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbConfig>
            (configuration.GetSection("Mongo"));
        services.AddScoped<IMongoDbClient, MongoDbClient>();
        
        return services;
    }
    
}