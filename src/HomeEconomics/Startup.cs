using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeEconomics
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
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
                .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment())
                .AddHomeEconomicsSwagger()
                .AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseHomeEconomicsSpa()
                .UseRouting()
                .UseHomeEconomicsCors()
                .UseHomeEconomicsSwagger()
                .UseProblemDetails()
                .UseHomeEconomicsEndpoints();
        }
    }
}
