using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HomeEconomics
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                    webHostBuilder.UseSerilog(ConfigureSerilog());
                    webHostBuilder.UseUrls(GetUrl());
                });
        }

        private static Action<WebHostBuilderContext, LoggerConfiguration> ConfigureSerilog()
        {
            return (webHostBuilderContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(webHostBuilderContext.Configuration);
            };
        }

        private static string GetUrl()
        {
            var port = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? 5000
                : 5001;

            return $"http://localhost:{port}";
        }
    }
}
