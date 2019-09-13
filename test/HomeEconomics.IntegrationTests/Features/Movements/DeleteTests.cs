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
    public class DeleteTests : IntegrationTestBase
    {
        private string _uri;

        public DeleteTests(Fixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task Should_Return_204_NoContent()
        {
            _uri = "api/movements/42";

            var response = await HttpClient
                .DeleteAsync(_uri);

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