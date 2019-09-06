using AutoMapper;
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
                .AddHomeEconomicsMvc()
                .AddHomeEconomicsMediatR()
                .AddHomeEconomicsAutoMapper()
                .AddHomeEconomicsPersistence(Configuration, Environment.IsDevelopment())
                .AddHomeEconomicsSwagger();
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment env)
        {
            applicationBuilder
                .UseHomeEconomicsSwagger()
                .UseProblemDetails()
                .UseMvc();
        }
    }
}
