using JetBrains.Annotations;
using LiteBus.Commands.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace HomeEconomics.IntegrationTests.Infrastructure;

[UsedImplicitly]
public sealed class Fixture : IDisposable, IAsyncDisposable
{
    private CustomWebApplicationFactory Factory { get; }
    private IServiceProvider ServiceProvider => Factory.Services;
    public HttpClient HttpClient { get; }
    public string ConnectionString { get; }
    
    public Fixture()
    {
        Factory = new CustomWebApplicationFactory();
        HttpClient = Factory.CreateClient();
        ConnectionString = PostgreSqlContainerManager.Instance.GetConnectionString();
        
        DeleteDatabase();
        MigrateDatabase();
    }
    
    public void Dispose()
    {
        HttpClient.Dispose();
        DeleteDatabase();
    }
    
    public async Task<TResponse> SendCommandToMediatorAsync<TResponse>(ICommand<TResponse> request)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ICommandMediator>();
        return await mediator!.SendAsync(request);
    }
    
    public async Task SendCommandToMediatorAsync(ICommand request)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ICommandMediator>();
        await mediator!.SendAsync(request);
    }
    
    public Task InsertDbContextAsync(params object[] entities) =>
        ExecuteDbContextAsync(dbContext =>
        {
            foreach (var entity in entities)
            {
                dbContext.Add(entity);
            }

            return dbContext.SaveChangesAsync();
        });
    
    public async Task<T> QueryDbContextAsync<T>(Func<HomeEconomicsDbContext, Task<T>> query) => await ExecuteDbContextAsync(query);
    
    private void DeleteDatabase()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HomeEconomicsDbContext>();
        dbContext.Database.EnsureDeleted();
    }
    
    private void MigrateDatabase()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HomeEconomicsDbContext>();
        dbContext.Database.Migrate();
    }
    
    private async Task<T> ExecuteDbContextAsync<T>(Func<HomeEconomicsDbContext, Task<T>> action)
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<HomeEconomicsDbContext>();
        return await action(dbContext!);
    }

    public async ValueTask DisposeAsync() => await PostgreSqlContainerManager.Instance.DisposeAsync();
}
