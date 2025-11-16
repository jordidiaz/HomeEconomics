using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class UpdateMonthMovementAmountTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private UpdateMonthMovementAmount.Command _command = new(1, 1, 50);

    private const string Uri = "api/movement-months/1/month-movements/1/update-amount";

    [Fact]
    public async Task Should_Return_200_Ok()
    {
        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Return_400_BadRequest()
    {
        _command = new UpdateMonthMovementAmount.Command(1, 1, -1);

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public class Handler : ICommandHandler<UpdateMonthMovementAmount.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> HandleAsync(UpdateMonthMovementAmount.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}
