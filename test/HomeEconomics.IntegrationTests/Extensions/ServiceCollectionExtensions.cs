using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using HomeEconomics.IntegrationTests.Infrastructure;
using LiteBus.Commands;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;

namespace HomeEconomics.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
    {
        services
            .AddProblemDetails(problemDetailsOptions =>
            {
                problemDetailsOptions.Map<InvalidOperationException>(_ => new StatusCodeProblemDetails(StatusCodes.Status409Conflict));
            })
            .AddMvcCore()
            .AddApplicationPart(typeof(HomeEconomicsApp).Assembly);
        services
            .AddFluentValidationAutoValidation(configuration =>
            {
                configuration.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblyContaining<HomeEconomicsApp>();
            
        return services;
    }

    internal static IServiceCollection AddHomeEconomicsMediator(this IServiceCollection services) =>
        services
            .AddLiteBus(liteBus =>
            {
                var appAssembly = typeof(TestStartup).Assembly;

                liteBus.AddCommandModule(module => module.RegisterFromAssembly(appAssembly));
                liteBus.AddQueryModule(module => module.RegisterFromAssembly(appAssembly));
            });
}
