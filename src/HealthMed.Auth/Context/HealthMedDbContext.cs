using HealthMed.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HealthMed.Auth.Context;

public class HealthMedDbContext : DbContext
{
    public HealthMedDbContext(DbContextOptions<HealthMedDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}



