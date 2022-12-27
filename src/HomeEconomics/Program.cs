using Persistence;
using Serilog;

namespace HomeEconomics
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.InitializeDbContext<HomeEconomicsDbContext>();

            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog(ConfigureSerilog())
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                    webHostBuilder.UseUrls(GetUrl());
                });
        }

        private static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilog()
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
                : 6001;

            return $"http://localhost:{port}";
        }
    }
}
