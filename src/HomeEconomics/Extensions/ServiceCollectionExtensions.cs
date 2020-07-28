using AutoMapper;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private const string SelfName = "self";

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

        internal static IServiceCollection AddHomeEconomicsHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HomeEconomics");

            services
                .AddHealthChecks()
                .AddCheck(name: SelfName, timeout: TimeSpan.FromSeconds(30), check: () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString)
                .AddApplicationInsightsPublisher();

            return services;
        }

        internal static IServiceCollection AddIf(this IServiceCollection services, bool condition, Func<IServiceCollection, IServiceCollection> action)
        {
            return condition ? action(services) : services;
        }
    }
}
