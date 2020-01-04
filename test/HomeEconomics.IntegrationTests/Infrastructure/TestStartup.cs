using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = HomeEconomics.IntegrationTests.Extensions.ServiceCollectionExtensions;

namespace HomeEconomics.IntegrationTests.Infrastructure
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollectionExtensions.AddHomeEconomicsApi(services);
            ServiceCollectionExtensions.AddHomeEconomicsMediatR(services);
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseRouting()
                .UseProblemDetails()
                .UseHomeEconomicsEndpoints();
        }
    }
}