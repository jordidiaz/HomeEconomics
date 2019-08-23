using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Persistence;
using Serilog;

namespace HomeEconomics
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            
            if (isService)
            {
                var mainModule = Process.GetCurrentProcess().MainModule;
                if (mainModule is null)
                {
                    throw new NoNullAllowedException(nameof(mainModule));
                }
                var pathToExe = mainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(
                args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            host.InitializeDbContext<HomeEconomicsDbContext>();

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(ConfigureSerilog())
                .UseApplicationInsights();
        }

        private static Action<WebHostBuilderContext, LoggerConfiguration> ConfigureSerilog()
        {
            return (webHostBuilderContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(webHostBuilderContext.Configuration);
            };
        }
    }
}
