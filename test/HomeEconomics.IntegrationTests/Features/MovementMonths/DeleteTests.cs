using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths;

public class DeleteTests : IntegrationTestBase
{
    private const string Uri = "api/movement-months/1/month-movements/1";

    public DeleteTests(Fixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return_200_Ok()
    {
        var response = await HttpClient
            .DeleteAsync(Uri);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : IRequestHandler<DeleteMonthMovement.Command, MovementMonthResponse>
    {
        public Task<MovementMonthResponse> Handle(DeleteMonthMovement.Command request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new MovementMonthResponse());
        }
    }
}