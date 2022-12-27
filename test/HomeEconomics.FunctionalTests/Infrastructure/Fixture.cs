using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Respawn;

namespace HomeEconomics.FunctionalTests.Infrastructure
{
    public static class Fixture
    {
        private static readonly IConfigurationRoot Configuration = GetConfigurationRoot();
        private static readonly ServiceProvider ServiceProvider = GetServiceProvider();
        private static readonly IServiceScopeFactory ScopeFactory = GetScopeFactory();
        private static readonly string ConnectionString = Configuration.GetConnectionString("HomeEconomics")!;
        private static readonly Respawner Respawner = Respawner.CreateAsync(ConnectionString).Result;

        static Fixture()
        {
            DeleteDatabase();
            MigrateDatabase();
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
            return ServiceProvider.GetService<IServiceScopeFactory>() ?? throw new ArgumentNullException("ServiceScopeFactory is null");
        }

        private static void MigrateDatabase()
        {
            var dbContext = ServiceProvider.GetService<HomeEconomicsDbContext>();
            dbContext?.Database.Migrate();
        }

        private static void DeleteDatabase()
        {
            var dbContext = ServiceProvider.GetService<HomeEconomicsDbContext>();
            dbContext?.Database.EnsureDeleted();
        }

        public static Task ResetCheckpointAsync() => Respawner.ResetAsync(ConnectionString);

        public static async Task<TResponse> SendToMediatRAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            if (mediator is null)
            {
                throw new ArgumentNullException("Mediator is null");
            }

            return await mediator.Send(request);
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
                if (dbContext is null)
                {
                    throw new ArgumentNullException("HomeEconomicsDbContext is null");
                }

                return await action(dbContext);
            }
        }
    }
}