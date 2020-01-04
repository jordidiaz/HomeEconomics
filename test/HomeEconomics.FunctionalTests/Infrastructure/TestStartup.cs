using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeEconomics.FunctionalTests.Infrastructure
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHomeEconomicsApi()
                .AddCors()
                .AddHomeEconomicsMediatR()
                .AddHomeEconomicsAutoMapper()
                .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment());
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseProblemDetails();
        }
    }
}
