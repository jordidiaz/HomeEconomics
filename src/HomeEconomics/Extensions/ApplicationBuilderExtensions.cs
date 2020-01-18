using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "spa/build");
            return appBuilder
                .UseDefaultFiles()
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), path))
                });
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
            return appBuilder.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());
        }
    }
}
