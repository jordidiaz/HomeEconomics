using Microsoft.AspNetCore.Mvc.Testing;

namespace HomeEconomics.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        PostgreSqlContainerManager.Instance.InitializeAsync().GetAwaiter().GetResult();
        
        builder.ConfigureServices((_, services) =>
        {
            var connectionString = PostgreSqlContainerManager.Instance.GetConnectionString();

            services.AddHomeEconomicsPersistence(connectionString, isDevelopment: true);
        });
        
        base.ConfigureWebHost(builder);
    }
}
