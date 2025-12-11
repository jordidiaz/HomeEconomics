using System.Net;
using FluentAssertions;
using HomeEconomics.IntegrationTests.Infrastructure;
using LiteBus.Queries.Abstractions;
using Xunit;
using Index = HomeEconomics.Features.Movements.Index;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class IndexTests(Fixture fixture) : IntegrationTestBase(fixture)
{
    private const string Uri = "api/movements";

    [Fact]
    public async Task Should_Return_200_And_Movements()
    {
        var response = await HttpClient.GetAsync(Uri);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : IQueryHandler<Index.Query, Index.Result>
    {
        public Task<Index.Result> HandleAsync(Index.Query request, CancellationToken cancellationToken = default) => Task.FromResult(new Index.Result([]));
    }
}
