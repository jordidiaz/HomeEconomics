using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class DeleteTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private const string Uri = "api/movements/42";

    [Fact]
    public async Task Should_Return_204_NoContent()
    {
        var response = await HttpClient
            .DeleteAsync(Uri);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public class Handler : ICommandHandler<Delete.Command>
    {
        public Task HandleAsync(Delete.Command request, CancellationToken cancellationToken) => Task.FromResult(Task.CompletedTask);
    }
}
