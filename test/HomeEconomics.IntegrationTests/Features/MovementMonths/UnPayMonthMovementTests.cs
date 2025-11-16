using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UnPayMonthMovementTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private const string Uri = "api/movement-months/1/month-movements/1/unpay";

    [Fact]
    public async Task Should_Return_200_Ok()
    {
        var response = await HttpClient
            .PostAsync(Uri, null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : ICommandHandler<UnPayMonthMovement.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> HandleAsync(UnPayMonthMovement.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}
