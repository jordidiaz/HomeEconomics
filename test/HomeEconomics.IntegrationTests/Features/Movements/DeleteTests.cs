using FluentAssertions;
using HomeEconomics.Features.Movements;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.Movements
{
    public class DeleteTests : IntegrationTestBase
    {
        private const string Uri = "api/movements/42";

        public DeleteTests(Fixture fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task Should_Return_204_NoContent()
        {
            var response = await HttpClient
                .DeleteAsync(Uri);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public class Handler : IRequestHandler<Delete.Command, Unit>
        {
            public Task<Unit> Handle(Delete.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Unit.Value);
            }
        }
    }
}