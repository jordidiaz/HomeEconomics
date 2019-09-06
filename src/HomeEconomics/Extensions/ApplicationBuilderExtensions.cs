using System;

namespace Microsoft.AspNetCore.Builder
{
    internal static class ApplicationBuilderExtensions
    {
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
