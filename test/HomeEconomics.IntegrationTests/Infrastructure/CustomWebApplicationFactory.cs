using Microsoft.AspNetCore.Mvc.Testing;

namespace HomeEconomics.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        PostgreSqlContainerManager.Instance.InitializeAsync().GetAwaiter().GetResult();

        var connectionString = PostgreSqlContainerManager.Instance.GetConnectionString();

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:HomeEconomics"] = connectionString
            });
        });

        builder.ConfigureServices((_, services) =>
        {
            services.AddHomeEconomicsPersistence(connectionString, isDevelopment: true);
        });

        base.ConfigureWebHost(builder);
    }
}
