using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEconomics.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHomeEconomicsMvc(this IServiceCollection services)
        {
            return services
                .AddProblemDetails()
                .AddMvcCore()
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                {
                    fluentValidationMvcConfiguration
                        .RegisterValidatorsFromAssemblyContaining<HomeEconomics>();
                    fluentValidationMvcConfiguration.ImplicitlyValidateChildProperties = true;
                    fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters()
                .Services;
        }

        internal static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(HomeEconomicsIntegrationTests));
        }
    }
}