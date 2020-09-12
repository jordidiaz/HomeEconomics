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

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHomeEconomicsApi()
                .AddHomeEconomicsServices()
                .AddIf(WebHostEnvironment.IsDevelopment(), serviceCollection => serviceCollection.AddCors())
                .AddHomeEconomicsMediatR()
                .AddHomeEconomicsAutoMapper()
                .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment())
                .AddHomeEconomicsSwagger()
                .AddApplicationInsightsTelemetry()
                .AddHomeEconomicsHealthChecks(Configuration);
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseHomeEconomicsSpa()
                .UseRouting()
                .UseIf(WebHostEnvironment.IsDevelopment(), appBuilder => appBuilder.UseHomeEconomicsCors())
                .UseHomeEconomicsSwagger()
                .UseProblemDetails()
                .UseHomeEconomicsEndpoints();
        }
    }
}
