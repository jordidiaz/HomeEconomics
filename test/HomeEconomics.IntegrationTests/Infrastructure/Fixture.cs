using JetBrains.Annotations;
using Xunit;

namespace HomeEconomics.IntegrationTests.Infrastructure;

[UsedImplicitly]
public sealed class Fixture : 
    ICollectionFixture<CustomWebApplicationFactory<TestStartup>>,
    IDisposable
{
    public HttpClient HttpClient { get; } = new CustomWebApplicationFactory<TestStartup>().CreateClient();

    public void Dispose() => HttpClient.Dispose();
}
