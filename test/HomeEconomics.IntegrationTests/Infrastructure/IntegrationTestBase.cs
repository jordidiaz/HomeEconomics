using Xunit;

namespace HomeEconomics.IntegrationTests.Infrastructure;

[Collection(Collections.IntegrationTestCollection)]
public abstract class IntegrationTestBase(Fixture fixture)
{
    protected HttpClient HttpClient => fixture.HttpClient;
}
