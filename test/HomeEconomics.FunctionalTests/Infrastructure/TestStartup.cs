using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeEconomics.FunctionalTests.Infrastructure;

public class TestStartup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
{
    private IConfiguration Configuration { get; } = configuration;
    private IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    public void ConfigureServices(IServiceCollection services) =>
        services
            .AddHomeEconomicsApi()
            .AddHomeEconomicsServices()
            .AddHomeEconomicsMediator()
            .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment());

    public void Configure(IApplicationBuilder applicationBuilder) =>
        applicationBuilder
            .UseProblemDetails();
}
