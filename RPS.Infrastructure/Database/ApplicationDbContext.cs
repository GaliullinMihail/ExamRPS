using RPS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RPS.Infrastructure.Database;

public sealed class ApplicationDbContext: IdentityDbContext<User>
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<Room> Rooms { get; set; }
    
    public new DbSet<Role> Roles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.Email)
            .Ignore(u => u.EmailConfirmed);
        
        builder.Entity<Role>().HasData(
            new Role
            {
                Id = "1",
                Name = "StandartUser",
                NormalizedName = "STANDARTUSER",
                LikesCountAllowed = 20,
                LocationViewAllowed = false
            });
        base.OnModelCreating(builder);
    }
}