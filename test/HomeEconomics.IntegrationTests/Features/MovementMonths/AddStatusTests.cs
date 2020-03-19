using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class AddStatusTests : IntegrationTestBase
    {
        private const string Uri = "api/movement-months/1/add-status";

        private readonly AddStatus.Command _command;

        public AddStatusTests(Fixture fixture) : base(fixture)
        {
            _command = new AddStatus.Command
            {
                Year = 2020,
                Month = Month.Jan,
                AccountAmount = 900,
                CashAmount = 50
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
            _command.Month = 0;

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<AddStatus.Command, AddStatus.Result>
        {
            public Task<AddStatus.Result> Handle(AddStatus.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new AddStatus.Result());
            }
        }
    }
}
