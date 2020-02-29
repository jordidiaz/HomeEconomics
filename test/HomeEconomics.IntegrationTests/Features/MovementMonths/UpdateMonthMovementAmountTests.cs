using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;
using System.Net.Http;
using FluentAssertions;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class UpdateMonthMovementAmountTests : IntegrationTestBase
    {
        private readonly UpdateMonthMovementAmount.Command _command;

        private const string Uri = "api/movement-months/1/month-movements/1/update-amount";

        public UpdateMonthMovementAmountTests(Fixture fixture) : base(fixture)
        {
            _command = new UpdateMonthMovementAmount.Command
            {
                MovementMonthId = 1,
                MonthMovementId = 1,
                Amount = 50
            };
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
            _command.Amount = -1;

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<UpdateMonthMovementAmount.Command, UpdateMonthMovementAmount.Result>
        {
            public Task<UpdateMonthMovementAmount.Result> Handle(UpdateMonthMovementAmount.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new UpdateMonthMovementAmount.Result());
            }
        }
    }
}
