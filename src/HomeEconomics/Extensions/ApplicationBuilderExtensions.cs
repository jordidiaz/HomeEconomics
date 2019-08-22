using System;

namespace Microsoft.AspNetCore.Builder
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseIf(this IApplicationBuilder appBuilder, bool condition, Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            return condition ? action(appBuilder) : appBuilder;
        }

        internal static IApplicationBuilder UseIfNot(this IApplicationBuilder appBuilder, bool condition, Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            return !condition ? action(appBuilder) : appBuilder;
        }

        internal static IApplicationBuilder UseHomeEconomicsSwagger(this IApplicationBuilder appBuilder)
        {
            return appBuilder
                .UseSwagger()
                .UseSwaggerUI(swaggerUiOptions =>
                {
                    swaggerUiOptions.SwaggerEndpoint("/swagger/hm/swagger.json", "HomeEconomics API");
                    swaggerUiOptions.RoutePrefix = string.Empty;
                });
        }
    }
}
