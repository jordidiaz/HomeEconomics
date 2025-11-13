using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class CreateTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private Create.Command _command = new(
        DateTime.Now.Year,
        Month.Feb);

    private const string Uri = "api/movement-months";

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
        _command = new Create.Command(
            DateTime.Now.Year - 1,
            Month.Feb);

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public class Handler : IRequestHandler<Create.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> Handle(Create.Command request, CancellationToken cancellationToken) => Task.FromResult(new MovementMonthResponse());
    }
}
