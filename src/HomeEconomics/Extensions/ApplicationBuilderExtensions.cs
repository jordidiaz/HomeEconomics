using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        private const string SelfName = "self";
        private const string Npgsql = "npgsql";

        internal static IApplicationBuilder UseHomeEconomicsSwagger(this IApplicationBuilder appBuilder)
        {
            return appBuilder
                .UseSwagger()
                .UseSwaggerUI(swaggerUiOptions =>
                {
                    swaggerUiOptions.SwaggerEndpoint("/swagger/hm/swagger.json", "HomeEconomics API");
                });
        }

        internal static IApplicationBuilder UseHomeEconomicsSpa(this IApplicationBuilder appBuilder)
        {
            return appBuilder
                .UseFileServer();
        }

        internal static IApplicationBuilder UseHomeEconomicsCors(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseCors(corsPolicyBuilder =>
                corsPolicyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        }

        internal static IApplicationBuilder UseHomeEconomicsEndpoints(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllers();
                endpointRouteBuilder.MapHealthChecks("/self", new HealthCheckOptions
                {
                    Predicate = registration => registration.Name == SelfName,
                });
                endpointRouteBuilder.MapHealthChecks("/npgsql", new HealthCheckOptions
                {
                    Predicate = registration => registration.Name == Npgsql,
                });
            });
        }

        internal static IApplicationBuilder UseIf(this IApplicationBuilder appBuilder, bool condition, Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            return condition ? action(appBuilder) : appBuilder;
        }
    }
}
