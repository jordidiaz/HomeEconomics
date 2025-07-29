using Domain.MovementMonth;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class HomeEconomicsDbContext : DbContext
{
    public HomeEconomicsDbContext(DbContextOptions options) : base(options)
    {
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<Movement> Movements { get; set; } = default!;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DbSet<MovementMonth> MovementMonths { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeEconomicsPersistence).Assembly);
    }
}