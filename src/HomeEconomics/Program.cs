using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("HomeEconomics");

builder.Services
    .AddHomeEconomicsApi()
    .AddHomeEconomicsServices()
    .AddIf(builder.Environment.IsDevelopment(), sc => sc.AddCors())
    .AddHomeEconomicsMediator()
    .AddHomeEconomicsPersistence(connectionString!, builder.Environment.IsDevelopment())
    .AddHomeEconomicsSwagger()
    .AddHomeEconomicsHealthChecks(connectionString!);

var app = builder.Build();

app
    .UseHomeEconomicsSpa()
    .UseRouting()
    .UseIf(app.Environment.IsDevelopment(), ab => ab.UseHomeEconomicsCors())
    .UseHomeEconomicsSwagger()
    .UseProblemDetails()
    .UseHomeEconomicsEndpoints();

await app.RunAsync();

// Don't remove this class, it's needed for the integration tests project to reference the HomeEconomics assembly.
public partial class Program { }
