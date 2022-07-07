using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class AddMonthMovementTests : IntegrationTestBase
    {
        private const string Uri = "api/movement-months/1/month-movements";

        private AddMonthMovement.Command _command;

        public AddMonthMovementTests(Fixture fixture) : base(fixture)
        {
            _command = new AddMonthMovement.Command(
                1,
                "Gasolina",
                60,
                MovementType.Expense);
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
            _command = new AddMonthMovement.Command(
                1,
                "Gasolina",
                -0.1m,
                MovementType.Expense);

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<AddMonthMovement.Command, MovementMonthResponse>
        {
            public Task<MovementMonthResponse> Handle(AddMonthMovement.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new MovementMonthResponse());
            }
        }
    }
}
