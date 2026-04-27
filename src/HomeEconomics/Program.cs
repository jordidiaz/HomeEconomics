using Hellang.Middleware.ProblemDetails;
using HomeEconomics.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

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

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    var connString = app.Configuration.GetConnectionString("HomeEconomics");
    await DevelopmentDatabaseBootstrapper.EnsureDatabaseExistsAsync(connString!);
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HomeEconomicsDbContext>();
    await dbContext.Database.MigrateAsync();
}

app
    .UseHomeEconomicsSpa()
    .UseRouting()
    .UseIf(app.Environment.IsDevelopment(), ab => ab.UseHomeEconomicsCors())
    .UseHomeEconomicsSwagger()
    .UseProblemDetails()
    .UseHomeEconomicsEndpoints();

await app.RunAsync();

// Don't remove this class! It's needed for the integration tests project to reference the HomeEconomics assembly.
public partial class Program { }
