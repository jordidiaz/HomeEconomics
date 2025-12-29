using Microsoft.EntityFrameworkCore;
using Persistence;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHomeEconomicsPersistence(
        this IServiceCollection services,
        string connectionString,
        bool isDevelopment)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            return services;
        }
        
        return services
            .AddDbContext<HomeEconomicsDbContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(
                        connectionString,
                        postgresDbContextOptionsBuilder => postgresDbContextOptionsBuilder.SetPostgresVersion(new Version(9, 6)))
                    .EnableSensitiveDataLogging(isDevelopment);
            });
    }
}
