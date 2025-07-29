using Hellang.Middleware.ProblemDetails;

namespace HomeEconomics;

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
            .AddHomeEconomicsPersistence(Configuration, WebHostEnvironment.IsDevelopment())
            .AddHomeEconomicsSwagger()
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