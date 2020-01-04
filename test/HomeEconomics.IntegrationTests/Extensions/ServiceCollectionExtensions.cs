using System;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEconomics.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
        {
            return services
                .AddProblemDetails(problemDetailsOptions =>
                {
                    problemDetailsOptions.Map<InvalidOperationException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status409Conflict));
                })
                .AddMvcCore()
                .AddApplicationPart(typeof(TestStartup).Assembly)
                .AddNewtonsoftJson()
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                {
                    fluentValidationMvcConfiguration
                        .RegisterValidatorsFromAssemblyContaining<HomeEconomics>();
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