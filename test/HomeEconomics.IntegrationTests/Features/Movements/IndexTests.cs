using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements
{
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
            public Task<Index.Result> Handle(Index.Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Index.Result());
            }
        }
    }
}