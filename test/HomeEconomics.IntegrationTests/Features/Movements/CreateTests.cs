using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class CreateTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private Create.Command _command = new("EPSV", 50m, MovementType.Expense,
        new Create.Frequency(FrequencyType.Monthly, 0, []));

    private const string Uri = "api/movements";

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
        _command = new Create.Command(string.Empty, 50m, MovementType.Expense, new Create.Frequency(FrequencyType.Monthly, 0, []));

        var response = await HttpClient
            .PostAsync(Uri, _command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public class Handler : ICommandHandler<Create.Command, int>
    {
        public Task<int> HandleAsync(Create.Command request, CancellationToken cancellationToken = default) => Task.FromResult(1);
    }
}
