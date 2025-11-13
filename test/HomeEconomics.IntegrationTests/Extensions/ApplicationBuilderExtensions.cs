// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class ApplicationBuilderExtensions
{
    internal static IApplicationBuilder UseHomeEconomicsEndpoints(this IApplicationBuilder appBuilder) => appBuilder.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());
}