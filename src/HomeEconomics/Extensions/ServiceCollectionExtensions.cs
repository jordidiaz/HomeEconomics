using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using HomeEconomics;
using HomeEconomics.Services;
using LiteBus.Commands;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string SelfName = "self";

    extension(IServiceCollection services)
    {
        public IServiceCollection AddHomeEconomicsApi()
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
                .AddValidatorsFromAssemblyContaining<HomeEconomicsApp>();
            
            return services;
        }

        public IServiceCollection AddHomeEconomicsMediator() =>
            services
                .AddLiteBus(liteBus =>
                {
                    var appAssembly = typeof(Program).Assembly;

                    liteBus.AddCommandModule(module => module.RegisterFromAssembly(appAssembly));
                    liteBus.AddQueryModule(module => module.RegisterFromAssembly(appAssembly));
                });

        public IServiceCollection AddHomeEconomicsServices() =>
            services
                .AddTransient<IMovementMonthResponseService, MovementMonthResponseService>();

        internal IServiceCollection AddHomeEconomicsSwagger() =>
            services
                .AddSwaggerGen(swaggerGenOptions =>
                {
                    swaggerGenOptions.SwaggerDoc("hm", new OpenApiInfo { Title = "HomeEconomics API" });
                    swaggerGenOptions.CustomSchemaIds(type => type.FullName);
                });

        internal IServiceCollection AddHomeEconomicsHealthChecks(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HomeEconomics");

            services
                .AddHealthChecks()
                .AddCheck(name: SelfName, timeout: TimeSpan.FromSeconds(30), check: () => HealthCheckResult.Healthy())
                .AddNpgSql(connectionString!);

            return services;
        }

        internal IServiceCollection AddIf(bool condition, Func<IServiceCollection, IServiceCollection> action) => condition ? action(services) : services;
    }
}
