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
                .UseSerilog(ConfigureSerilog())
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                    var port = GetPort();
                    var dockerPort = port + 1;
                    webHostBuilder.UseUrls($"http://localhost:{port}", $"http://0.0.0.0:{dockerPort}");
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

        private static int GetPort()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? 5000
                : 6001;
        }
    }
}
