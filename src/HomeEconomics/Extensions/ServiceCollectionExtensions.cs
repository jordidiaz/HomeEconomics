using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using HomeEconomics.Services;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string SelfName = "self";

    public static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
    {
        services
            .AddProblemDetails(problemDetailsOptions =>
            {
                problemDetailsOptions.Map<InvalidOperationException>(_ =>
                    new StatusCodeProblemDetails(StatusCodes.Status409Conflict));
            })
            .AddMvcCore()
            .AddApiExplorer();

        services
            .AddFluentValidationAutoValidation(configuration =>
            {
                configuration.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblyContaining<HomeEconomics.HomeEconomicsApp>();
            
        return services;
    }

    public static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services) =>
        services
            .AddMediatR(typeof(HomeEconomics.HomeEconomicsApp));

    public static IServiceCollection AddHomeEconomicsServices(this IServiceCollection services) =>
        services
            .AddTransient<IMovementMonthResponseService, MovementMonthResponseService>();

    internal static IServiceCollection AddHomeEconomicsSwagger(this IServiceCollection services) =>
        services
            .AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("hm", new OpenApiInfo { Title = "HomeEconomics API" });
                swaggerGenOptions.CustomSchemaIds(type => type.FullName);
            });

    internal static IServiceCollection AddHomeEconomicsHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HomeEconomics");

        services
            .AddHealthChecks()
            .AddCheck(name: SelfName, timeout: TimeSpan.FromSeconds(30), check: () => HealthCheckResult.Healthy())
            .AddNpgSql(connectionString!);

        return services;
    }

    internal static IServiceCollection AddIf(this IServiceCollection services, bool condition, Func<IServiceCollection, IServiceCollection> action) => condition ? action(services) : services;
}
