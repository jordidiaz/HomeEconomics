using System.Net;
using FluentAssertions;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;
using Index = HomeEconomics.Features.Movements.Index;

namespace HomeEconomics.IntegrationTests.Features.Movements;

public class IndexTests : IntegrationTestBase
{
    private const string Uri = "api/movements";

    public IndexTests(Fixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return_200_And_Movements()
    {
        var response = await HttpClient.GetAsync(Uri);

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public class Handler : IRequestHandler<Index.Query, Index.Result>
    {
        public Task<Index.Result> Handle(Index.Query request, CancellationToken cancellationToken) => Task.FromResult(new Index.Result());
    }
}