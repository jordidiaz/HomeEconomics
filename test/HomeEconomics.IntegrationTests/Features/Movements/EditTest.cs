using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using System.Net;
using LiteBus.Commands.Abstractions;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class EditTest(Fixture fixture) : IntegrationTestBase(fixture)
{
    private readonly Edit.Command _command = new()
    {
        Id = 42,
        Name = "EPSV",
        Amount = 50m,
        Type = MovementType.Expense,
        Frequency = new Edit.Frequency
        {
            Type = FrequencyType.Monthly
        }
    };

    private const string Uri = "api/movements/42";

    [Fact]
    public async Task Should_Return_204_NoContent()
    {
        var response = await HttpClient
            .PutAsync(Uri, _command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public class Handler : ICommandHandler<Edit.Command>
    {
        public Task HandleAsync(Edit.Command request, CancellationToken cancellationToken) => Task.FromResult(Task.CompletedTask);
    }
}
