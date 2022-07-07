using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class UpdateMonthMovementAmountTests : IntegrationTestBase
    {
        private UpdateMonthMovementAmount.Command _command;

        private const string Uri = "api/movement-months/1/month-movements/1/update-amount";

        public UpdateMonthMovementAmountTests(Fixture fixture) : base(fixture)
        {
            _command = new UpdateMonthMovementAmount.Command(1, 1, 50);
        }

        [Fact]
        public async Task Should_Return_200_Ok()
        {
            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_400_BadRequest()
        {
            _command = new UpdateMonthMovementAmount.Command(1, 1, -1);

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<UpdateMonthMovementAmount.Command, MovementMonthResponse>
        {
            public Task<MovementMonthResponse> Handle(UpdateMonthMovementAmount.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new MovementMonthResponse());
            }
        }
    }
}
