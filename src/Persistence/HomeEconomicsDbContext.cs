using Domain.MovementMonth;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class HomeEconomicsDbContext(DbContextOptions options) : DbContext(options)
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<Movement> Movements { get; set; } = null!;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<MovementMonth> MovementMonths { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeEconomicsPersistence).Assembly);
    }
}
