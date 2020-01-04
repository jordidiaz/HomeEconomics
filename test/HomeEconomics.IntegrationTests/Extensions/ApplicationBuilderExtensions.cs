namespace Microsoft.AspNetCore.Builder
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseHomeEconomicsEndpoints(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseEndpoints(endpointRouteBuilder => endpointRouteBuilder.MapControllers());
        }
    }
}