using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEconomics
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHomeEconomicsApplicationInsights()
                .AddHomeEconomicsMvc()
                .AddHomeEconomicsMediatR()
                .AddHomeEconomicsPersistence(Configuration, Environment.IsDevelopment())
                .AddIf(Environment.IsDevelopment(), serviceCollection => serviceCollection.AddHomeEconomicsSwagger());
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment env)
        {
            applicationBuilder
                .UseIfNot(env.IsDevelopment(), appBuilder => appBuilder.UseHsts())
                .UseHttpsRedirection()
                .UseIf(env.IsDevelopment(), appBuilder => appBuilder.UseHomeEconomicsSwagger())
                .UseProblemDetails()
                .UseMvc();
        }
    }
}
