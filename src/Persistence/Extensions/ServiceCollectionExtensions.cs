using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Persistence;

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
                        .ConfigureWarnings(warningsConfigurationBuilder =>
                            warningsConfigurationBuilder.Throw(RelationalEventId.QueryClientEvaluationWarning))
                        .EnableSensitiveDataLogging(isDevelopment);
                });
        }
    }
}
