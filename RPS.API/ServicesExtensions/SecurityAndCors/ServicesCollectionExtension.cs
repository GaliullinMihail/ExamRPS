namespace RPS.API.ServicesExtensions.SecurityAndCors;

public static class ServicesCollectionExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, string testSpecific)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: testSpecific, policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
        });
        
        return services;
    }
}