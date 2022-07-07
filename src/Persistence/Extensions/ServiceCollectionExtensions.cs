using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeEconomicsPersistence(
            this IServiceCollection services,
            IConfiguration configuration,
            bool isDevelopment)
        {
            var connectionString = configuration.GetConnectionString("HomeEconomics");

            return services
                .AddDbContext<HomeEconomicsDbContext>(dbContextOptionsBuilder =>
                {
                    dbContextOptionsBuilder
                        .UseSqlServer(
                            connectionString,
                            sqlServerDbContextOptionsBuilder => sqlServerDbContextOptionsBuilder.EnableRetryOnFailure())
                        .EnableSensitiveDataLogging(isDevelopment);
                });
        }
    }
}
