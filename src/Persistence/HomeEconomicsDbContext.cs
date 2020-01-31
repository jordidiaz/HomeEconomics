using Domain.MovementMonth;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class HomeEconomicsDbContext : DbContext
    {
        public HomeEconomicsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movement> Movements { get; set; }

        public DbSet<MovementMonth> MovementMonths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeEconomicsPersistence).Assembly);
        }
    }
}
