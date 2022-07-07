using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;

namespace HomeEconomics.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
        {
            return services
                .AddProblemDetails(problemDetailsOptions =>
                {
                    problemDetailsOptions.Map<InvalidOperationException>(_ => new StatusCodeProblemDetails(StatusCodes.Status409Conflict));
                })
                .AddMvcCore()
                .AddApplicationPart(typeof(HomeEconomicsApp).Assembly)
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                {
                    fluentValidationMvcConfiguration
                        .RegisterValidatorsFromAssemblyContaining<HomeEconomicsApp>();
                    fluentValidationMvcConfiguration.ImplicitlyValidateChildProperties = true;
                    fluentValidationMvcConfiguration.DisableDataAnnotationsValidation = true;
                })
                .Services;
        }

        internal static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(HomeEconomicsIntegrationTests));
        }
    }
}