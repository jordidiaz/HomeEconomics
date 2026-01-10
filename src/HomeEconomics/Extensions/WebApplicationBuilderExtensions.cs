using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder webApplicationBuilder)
    {
        internal WebApplicationBuilder UseSerilog()
        {
            webApplicationBuilder.Logging.ClearProviders();
            webApplicationBuilder.Host.UseSerilog((hostBuilderContext, _, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
            }); 
            
            return webApplicationBuilder;
        }
    }
}
