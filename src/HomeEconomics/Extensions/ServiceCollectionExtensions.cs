using AutoMapper;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeEconomicsApi(this IServiceCollection services)
        {
            return services
                .AddProblemDetails(problemDetailsOptions =>
                {
                    problemDetailsOptions.Map<InvalidOperationException>(ex => new StatusCodeProblemDetails(StatusCodes.Status409Conflict));
                })
                .AddMvcCore()
                .AddNewtonsoftJson()
                .AddFluentValidation(fluentValidationMvcConfiguration =>
                    {
                        fluentValidationMvcConfiguration
                            .RegisterValidatorsFromAssemblyContaining<HomeEconomics.HomeEconomicsApp>();
                        fluentValidationMvcConfiguration.ImplicitlyValidateChildProperties = true;
                        fluentValidationMvcConfiguration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    })
                .AddApiExplorer()
                .Services;
        }

        public static IServiceCollection AddHomeEconomicsMediatR(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(HomeEconomics.HomeEconomicsApp));
        }

        public static IServiceCollection AddHomeEconomicsAutoMapper(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(typeof(HomeEconomics.HomeEconomicsApp));
        }

        internal static IServiceCollection AddHomeEconomicsSwagger(this IServiceCollection services)
        {
            return services
                .AddSwaggerGen(swaggerGenOptions =>
                {
                    swaggerGenOptions.SwaggerDoc("hm", new OpenApiInfo { Title = "HomeEconomics API" });
                    swaggerGenOptions.CustomSchemaIds(type => type.FullName);
                });
        }

        internal static IServiceCollection AddIf(this IServiceCollection services, bool condition, Func<IServiceCollection, IServiceCollection> action)
        {
            return condition ? action(services) : services;
        }
    }
}
