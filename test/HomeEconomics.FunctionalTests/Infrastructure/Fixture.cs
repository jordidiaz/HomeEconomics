using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Respawn;

namespace HomeEconomics.FunctionalTests.Infrastructure
{
    public static class Fixture
    {
        private static readonly Checkpoint Checkpoint = new Checkpoint();
        private static readonly IConfigurationRoot Configuration = GetConfigurationRoot();
        private static readonly ServiceProvider ServiceProvider = GetServiceProvider();
        private static readonly IServiceScopeFactory ScopeFactory = GetScopeFactory();

        static Fixture()
        {
            CreateDatabase();
        }

        private static ServiceProvider GetServiceProvider()
        {
            var webHostEnvironment = A.Fake<IWebHostEnvironment>();
            var startup = new TestStartup(Configuration, webHostEnvironment);
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();
        }

        private static IServiceScopeFactory GetScopeFactory()
        {
            return ServiceProvider.GetService<IServiceScopeFactory>();
        }

        private static void CreateDatabase()
        {
            var dbContext = ServiceProvider.GetService<HomeEconomicsDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public static Task ResetCheckpointAsync() => Checkpoint.Reset(Configuration.GetConnectionString("HomeEconomics"));

        public static async Task<TResponse> SendToMediatRAsync<TResponse>(IRequest<TResponse> request)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                return await mediator.Send(request);
            }
        }

        public static async Task<T> QueryDbContextAsync<T>(Func<HomeEconomicsDbContext, Task<T>> query)
        {
            return await ExecuteDbContextAsync(query);
        }

        public static Task InsertDbContextAsync(params object[] entities)
        {
            return ExecuteDbContextAsync(dbContext =>
            {
                foreach (var entity in entities)
                {
                    dbContext.Add(entity);
                }

                return dbContext.SaveChangesAsync();
            });
        }

        private static async Task<T> ExecuteDbContextAsync<T>(Func<HomeEconomicsDbContext, Task<T>> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<HomeEconomicsDbContext>();

                return await action(dbContext);
            }
        }
    }
}