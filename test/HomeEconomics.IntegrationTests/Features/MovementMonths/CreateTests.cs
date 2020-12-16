using Domain.MovementMonth;
using FluentAssertions;
using HomeEconomics.Features.MovementMonths;
using HomeEconomics.IntegrationTests.Infrastructure;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HomeEconomics.IntegrationTests.Features.MovementMonths
{
    public class CreateTests : IntegrationTestBase
    {
        private Create.Command _command;

        private const string Uri = "api/movement-months";

        public CreateTests(Fixture fixture) : base(fixture)
        {
            _command = new Create.Command
            {
                Year = 2020,
                Month = Month.Feb
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
            _command = new Create.Command
            {
                Year = 2019,
                Month = Month.Feb
            };

            var response = await HttpClient
                .PostAsync(Uri, _command);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public class Handler : IRequestHandler<Create.Command, MovementMonthResponse>
        {
            public Task<MovementMonthResponse> Handle(Create.Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new MovementMonthResponse());
            }
        }
    }
}
