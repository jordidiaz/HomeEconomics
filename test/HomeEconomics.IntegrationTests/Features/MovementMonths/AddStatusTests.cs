using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class AddStatusTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private const string Uri = "api/movement-months/1/add-status";

    private AddStatus.Command _command = new(
        DateTime.Now.Year,
        Month.Jan,
        900,
        50);

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
        _command = new AddStatus.Command(
            DateTime.Now.Year - 1,
            0,
            900,
            50);

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public class Handler : ICommandHandler<AddStatus.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> HandleAsync(AddStatus.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}
