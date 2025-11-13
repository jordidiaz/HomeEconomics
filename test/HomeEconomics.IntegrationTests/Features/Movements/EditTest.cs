using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class EditTest : IntegrationTestBase
{
    private readonly Edit.Command _command;

    private const string Uri = "api/movements/42";

    public EditTest(Fixture fixture) : base(fixture) =>
        _command = new Edit.Command
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

    [Fact]
    public async Task Should_Return_204_NoContent()
    {
        var response = await HttpClient
            .PutAsync(Uri, _command);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public class Handler : IRequestHandler<Edit.Command, Unit>
    {
        public Task<Unit> Handle(Edit.Command request, CancellationToken cancellationToken) => Task.FromResult(Unit.Value);
    }
}