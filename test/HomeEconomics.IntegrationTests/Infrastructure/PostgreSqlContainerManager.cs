using Testcontainers.PostgreSql;

namespace HomeEconomics.IntegrationTests.Infrastructure;

public class PostgreSqlContainerManager : IAsyncDisposable
{
    private static readonly Lazy<PostgreSqlContainerManager> LazyInstance = new(() => new PostgreSqlContainerManager());
    private PostgreSqlContainer _container = null!;
    private string Hostname => _container.Hostname;
    private int Port => _container.GetMappedPublicPort(5432);
    private const string Username = PostgreSqlBuilder.DefaultUsername;
    private const string Password = PostgreSqlBuilder.DefaultPassword;
    
    public const string DatabaseName = "HomeEconomics";

    public static PostgreSqlContainerManager Instance => LazyInstance.Value;

    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:9.6.24-bullseye")
            .WithName(DatabaseName)
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }
        

    public async ValueTask DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
    
    public string GetConnectionString() => $"Host={Hostname};Port={Port};Database={DatabaseName};Username={Username};Password={Password}";
}
