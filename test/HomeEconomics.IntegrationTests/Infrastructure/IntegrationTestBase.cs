using Xunit;

namespace HomeEconomics.IntegrationTests.Infrastructure
{
    [Collection(Collections.IntegrationTestCollection)]
    public abstract class IntegrationTestBase
    {
        private readonly Fixture _fixture;

        protected IntegrationTestBase(Fixture fixture)
        {
            _fixture = fixture;
        }

        protected HttpClient HttpClient => _fixture.HttpClient;
    }
}