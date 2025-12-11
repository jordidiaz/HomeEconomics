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
    private readonly Edit.Command _command = new(42, "EPSV", 50m, MovementType.Expense, new Edit.Frequency(FrequencyType.Monthly, 0, []));

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
        public Task HandleAsync(Edit.Command request, CancellationToken cancellationToken = default) => Task.FromResult(Task.CompletedTask);
    }
}
