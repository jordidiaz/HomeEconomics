using System;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHomeEconomicsMvc(this IServiceCollection services)
        {
            return services
                .AddProblemDetails()
                .AddMvcCore()
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                    {
                        fluentValidationMvcConfiguration
                            .RegisterValidatorsFromAssemblyContaining<HomeEconomics.HomeEconomics>();
                        fluentValidationMvcConfiguration.ImplicitlyValidateChildProperties = true;
                        fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    })
                .AddApiExplorer()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters()
                .Services;
        }

        internal static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(HomeEconomics.HomeEconomics));
        }

        internal static IServiceCollection AddHomeEconomicsApplicationInsights(this IServiceCollection services)
        {
            return services
                .AddApplicationInsightsTelemetry();
        }

        internal static IServiceCollection AddHomeEconomicsSwagger(this IServiceCollection services)
        {
            return services
                .AddSwaggerGen(swaggerGenOptions =>
                {
                    swaggerGenOptions.SwaggerDoc("hm", new Info { Title = "HomeEconomics API" });
                });
        }

        internal static IServiceCollection AddIf(this IServiceCollection services, bool condition, Func<IServiceCollection, IServiceCollection> action)
        {
            return condition ? action(services) : services;
        }
    }
}
