using RPS.API.Hubs;
using RPS.API.ServicesExtensions.Auth;
using RPS.API.ServicesExtensions.MassTransit;
using RPS.API.ServicesExtensions.SecurityAndCors;
using RPS.API.ServicesExtensions.Services;
using RPS.API.ServicesExtensions.Swagger;
using RPS.Application.Helpers;
using RPS.Domain.Entities;
using RPS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RPS.API.ServicesExtensions.Mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Configuration.AddEnvironmentVariables();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
 options.UseSqlServer(builder.Configuration.GetConnectionString("RPSDatabase"));
 options.EnableSensitiveDataLogging();
});
builder.Services.AddIdentity<User, Role>(
     options =>
     {
         options.SignIn.RequireConfirmedAccount = false; // change in prod
         options.SignIn.RequireConfirmedEmail = true;  // change in prod
     })
 .AddDefaultTokenProviders()
 .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(
 o => o.TokenLifespan = TimeSpan.FromHours(24));

builder.Services.AddCustomServices(builder.Configuration);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(AplicationAssemblyReference.Assembly);
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddCustomAuth(builder.Configuration);
builder.Services.AddMasstransitRabbitMq(builder.Configuration);
builder.Services.AddMongo(builder.Configuration);
builder.Services.AddCustomSwaggerGenerator();

const string testSpecific = "testSpecific";

builder.Services.AddRouting(options =>
{
 options.LowercaseUrls = true;
 options.LowercaseQueryStrings = false;
});

builder.Services.AddCustomCors(testSpecific);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.MapHub<ChatHub>("/chatHub");

app.UseCors(testSpecific);

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();