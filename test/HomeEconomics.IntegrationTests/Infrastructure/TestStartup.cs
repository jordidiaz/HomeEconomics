using Hellang.Middleware.ProblemDetails;
using HomeEconomics.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEconomics.IntegrationTests.Infrastructure
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHomeEconomicsMvc()
                .AddHomeEconomicsMediatR();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseProblemDetails()
                .UseMvc();
        }
    }
}