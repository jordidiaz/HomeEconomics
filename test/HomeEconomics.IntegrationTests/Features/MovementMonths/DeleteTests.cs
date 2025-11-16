using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class DeleteTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private const string Uri = "api/movement-months/1/month-movements/1";

    [Fact]
    public async Task Should_Return_200_Ok()
    {
        var response = await HttpClient
            .DeleteAsync(Uri);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : ICommandHandler<DeleteMonthMovement.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> HandleAsync(DeleteMonthMovement.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}
