using Xunit;

namespace HomeEconomics.IntegrationTests.Infrastructure;

public sealed class Fixture : 
    ICollectionFixture<CustomWebApplicationFactory<TestStartup>>,
    IDisposable
{
    public Fixture() => HttpClient = new CustomWebApplicationFactory<TestStartup>().CreateClient();

    public HttpClient HttpClient { get; }

    public void Dispose() => HttpClient.Dispose();
}