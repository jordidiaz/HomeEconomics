using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    private const string SelfName = "self";
    private const string Npgsql = "npgsql";

    extension(IApplicationBuilder appBuilder)
    {
        internal IApplicationBuilder UseHomeEconomicsSwagger() =>
            appBuilder
                .UseSwagger()
                .UseSwaggerUI(swaggerUiOptions =>
                {
                    swaggerUiOptions.SwaggerEndpoint("/swagger/hm/swagger.json", "HomeEconomics API");
                });

        internal IApplicationBuilder UseHomeEconomicsSpa() =>
            appBuilder
                .UseFileServer();

        internal IApplicationBuilder UseHomeEconomicsCors() =>
            appBuilder.UseCors(corsPolicyBuilder =>
                corsPolicyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

        internal IApplicationBuilder UseHomeEconomicsEndpoints() =>
            appBuilder.UseEndpoints(endpointRouteBuilder =>
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

        internal IApplicationBuilder UseIf(bool condition, Func<IApplicationBuilder, IApplicationBuilder> action) => condition ? action(appBuilder) : appBuilder;
    }
}
