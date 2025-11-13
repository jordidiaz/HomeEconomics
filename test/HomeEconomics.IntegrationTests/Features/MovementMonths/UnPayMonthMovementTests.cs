using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UnPayMonthMovementTests : IntegrationTestBase
{
    private const string Uri = "api/movement-months/1/month-movements/1/unpay";

    public UnPayMonthMovementTests(Fixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return_200_Ok()
    {
        var response = await HttpClient
            .PostAsync(Uri, null!);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : IRequestHandler<UnPayMonthMovement.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> Handle(UnPayMonthMovement.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}