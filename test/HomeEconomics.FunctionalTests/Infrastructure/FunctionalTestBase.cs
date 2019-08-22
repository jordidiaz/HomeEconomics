using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.FunctionalTests.Infrastructure
{
    public class FunctionalTestBase : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await Fixture.ResetCheckpointAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
