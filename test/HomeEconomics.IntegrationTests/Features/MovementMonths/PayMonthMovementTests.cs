using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class PayMonthMovementTests : IntegrationTestBase
    {
        private const string Uri = "api/movement-months/1/month-movements/1/pay";

        public PayMonthMovementTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_Return_200_Ok()
        {
            var response = await HttpClient
                .PostAsync(Uri, null!);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public class Handler : IRequestHandler<PayMonthMovement.Command, MovementMonthResponse>
        {
            public Task<MovementMonthResponse> Handle(PayMonthMovement.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new MovementMonthResponse());
            }
        }
    }
}
