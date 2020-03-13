using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Movements;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class AddMonthMovementTests : IntegrationTestBase
    {
        private const string Uri = "api/movement-months/1/month-movements";

        private readonly AddMonthMovement.Command _command;

        public AddMonthMovementTests(Fixture fixture) : base(fixture)
        {
            _command = new AddMonthMovement.Command
            {
                MovementMonthId = 1,
                Name = "Gasolina",
                Amount = 60,
                Type = MovementType.Expense
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
            _command.Amount = -0.1m;

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<AddMonthMovement.Command, AddMonthMovement.Result>
        {
            public Task<AddMonthMovement.Result> Handle(AddMonthMovement.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new AddMonthMovement.Result());
            }
        }
    }
}
