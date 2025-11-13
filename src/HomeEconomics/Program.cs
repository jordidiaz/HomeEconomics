using Persistence;
using Serilog;

namespace HomeEconomics;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.InitializeDbContext<HomeEconomicsDbContext>();
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog(ConfigureSerilog())
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            });

    private static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilog() =>
        (webHostBuilderContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(webHostBuilderContext.Configuration);
        };
}
