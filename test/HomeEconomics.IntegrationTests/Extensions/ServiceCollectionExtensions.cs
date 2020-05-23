using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HomeEconomics.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
        {
            return services
                .AddProblemDetails(problemDetailsOptions =>
                {
                    problemDetailsOptions.Map<InvalidOperationException>(ex => new StatusCodeProblemDetails(StatusCodes.Status409Conflict));
                })
                .AddMvcCore()
                .AddApplicationPart(typeof(TestStartup).Assembly)
                .AddNewtonsoftJson()
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                {
                    fluentValidationMvcConfiguration
                        .RegisterValidatorsFromAssemblyContaining<HomeEconomicsApp>();
                    fluentValidationMvcConfiguration.ImplicitlyValidateChildProperties = true;
                    fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
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