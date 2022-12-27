using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;

namespace HomeEconomics.IntegrationTests.Extensions
{
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

        internal static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(HomeEconomicsIntegrationTests));
        }
    }
}