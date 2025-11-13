using Hellang.Middleware.ProblemDetails;

namespace HomeEconomics;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
{
    private IConfiguration Configuration { get; } = configuration;
    private IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    public void ConfigureServices(IServiceCollection services) =>
        services
            .AddHomeEconomicsApi()
            .AddHomeEconomicsServices()
            .AddIf(WebHostEnvironment.IsDevelopment(), serviceCollection => serviceCollection.AddCors())
            .AddHomeEconomicsMediatR()
            .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment())
            .AddHomeEconomicsSwagger()
            .AddHomeEconomicsHealthChecks(Configuration);

    public void Configure(IApplicationBuilder applicationBuilder) =>
        applicationBuilder
            .UseHomeEconomicsSpa()
            .UseRouting()
            .UseIf(WebHostEnvironment.IsDevelopment(), appBuilder => appBuilder.UseHomeEconomicsCors())
            .UseHomeEconomicsSwagger()
            .UseProblemDetails()
            .UseHomeEconomicsEndpoints();
}
