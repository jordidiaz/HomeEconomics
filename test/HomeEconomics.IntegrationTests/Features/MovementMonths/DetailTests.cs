using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class DetailTests : IntegrationTestBase
    {
        private const string Uri2020 = "api/movement-months/2020/2";
        private const string Uri2019 = "api/movement-months/2019/2";

        public DetailTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_Return_200_Ok()
        {
            var response = await HttpClient
                .GetAsync(Uri2020);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_404_NotFound()
        {
            var response = await HttpClient
                .GetAsync(Uri2019);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public class Handler : IRequestHandler<Detail.Query, MovementMonthResponse>
        {
            public Task<MovementMonthResponse> Handle(Detail.Query request, CancellationToken cancellationToken)
            {
                return request.Year == 2020 ? Task.FromResult(new MovementMonthResponse()) : Task.FromResult<MovementMonthResponse>(null);
            }
        }
    }
}
